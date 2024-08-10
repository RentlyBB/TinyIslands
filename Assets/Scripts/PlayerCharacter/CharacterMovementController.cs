using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;

namespace PlayerCharacter {
    public class CharacterMovementController : MonoBehaviour, ICharacterController {
        public KinematicCharacterMotor Motor;

        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;

        public float StableMovementSharpness = 15f;
        public float OrientationSharpness = 10f;
        public OrientationMethod OrientationMethod = OrientationMethod.TowardsCamera;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 15f;

        public float AirAccelerationSpeed = 15f;
        public float Drag = 0.1f;

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new();

        public BonusOrientationMethod BonusOrientationMethod = BonusOrientationMethod.None;
        public float BonusOrientationSharpness = 10f;
        public Vector3 Gravity = new(0, -30f, 0);
        public Transform MeshRoot;
        public Transform CameraFollowPoint;

        private Vector3 _internalVelocityAdd = Vector3.zero;
        private Vector3 _lookInputVector;
        private Vector3 _moveInputVector;

        private Collider[] _probedColliders = new Collider[8];
        private RaycastHit[] _probedHits = new RaycastHit[8];

        private Quaternion _tmpTransientRot;

        private Vector3 lastInnerNormal = Vector3.zero;
        private Vector3 lastOuterNormal = Vector3.zero;

        public CharacterState CurrentCharacterState { get; private set; }

        private void Awake() {
            // Handle initial state
            TransitionToState(CharacterState.Default);

            // Assign the characterController to the motor
            Motor.CharacterController = this;
        }

        /// <summary>
        ///     (Called by KinematicCharacterMotor during its update cycle)
        ///     This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime) { }

        /// <summary>
        ///     (Called by KinematicCharacterMotor during its update cycle)
        ///     This is where you tell your character what its rotation should be right now.
        ///     This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    if (_lookInputVector.sqrMagnitude > 0f && OrientationSharpness > 0f) {
                        // Smoothly interpolate from current to target look direction
                        var smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector,
                            1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                        // Set the current rotation (which will be used by the KinematicCharacterMotor)
                        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                    }

                    var currentUp = currentRotation * Vector3.up;
                    if (BonusOrientationMethod == BonusOrientationMethod.TowardsGravity) {
                        // Rotate from current up to invert gravity
                        var smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized,
                            1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                    } else if (BonusOrientationMethod == BonusOrientationMethod.TowardsGroundSlopeAndGravity) {
                        if (Motor.GroundingStatus.IsStableOnGround) {
                            var initialCharacterBottomHemiCenter =
                                Motor.TransientPosition + currentUp * Motor.Capsule.radius;

                            var smoothedGroundNormal = Vector3.Slerp(Motor.CharacterUp,
                                Motor.GroundingStatus.GroundNormal, 1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGroundNormal) * currentRotation;

                            // Move the position to create a rotation around the bottom hemi center instead of around the pivot
                            Motor.SetTransientPosition(initialCharacterBottomHemiCenter +
                                                       currentRotation * Vector3.down * Motor.Capsule.radius);
                        } else {
                            var smoothedGravityDir = Vector3.Slerp(currentUp, -Gravity.normalized,
                                1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                            currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                        }
                    } else {
                        var smoothedGravityDir = Vector3.Slerp(currentUp, Vector3.up,
                            1 - Mathf.Exp(-BonusOrientationSharpness * deltaTime));
                        currentRotation = Quaternion.FromToRotation(currentUp, smoothedGravityDir) * currentRotation;
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///     (Called by KinematicCharacterMotor during its update cycle)
        ///     This is where you tell your character what its velocity should be right now.
        ///     This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime) {
            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    // Ground movement
                    if (Motor.GroundingStatus.IsStableOnGround) {
                        var currentVelocityMagnitude = currentVelocity.magnitude;

                        var effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                        // Reorient velocity on slope
                        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                                          currentVelocityMagnitude;

                        // Calculate target velocity
                        var inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                        var reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                              _moveInputVector.magnitude;
                        var targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                        // Smooth movement Velocity
                        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity,
                            1f - Mathf.Exp(-StableMovementSharpness * deltaTime));
                    }
                    // Air movement
                    else {
                        // Add move input
                        if (_moveInputVector.sqrMagnitude > 0f) {
                            var addedVelocity = deltaTime * AirAccelerationSpeed * _moveInputVector;

                            var currentVelocityOnInputsPlane =
                                Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                            // Limit air velocity from inputs
                            if (currentVelocityOnInputsPlane.magnitude < MaxAirMoveSpeed) {
                                // clamp addedVel to make total vel not exceed max vel on inputs plane
                                var newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity,
                                    MaxAirMoveSpeed);
                                addedVelocity = newTotal - currentVelocityOnInputsPlane;
                            } else {
                                // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                                if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity,
                                        currentVelocityOnInputsPlane.normalized);
                            }

                            // Prevent air-climbing sloped walls
                            if (Motor.GroundingStatus.FoundAnyGround)
                                if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f) {
                                    var perpenticularObstructionNormal = Vector3
                                        .Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal),
                                            Motor.CharacterUp).normalized;
                                    addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                                }

                            // Apply added velocity
                            currentVelocity += addedVelocity;
                        }

                        // Gravity
                        currentVelocity += Gravity * deltaTime;

                        // Drag
                        currentVelocity *= 1f / (1f + Drag * deltaTime);
                    }

                    // Take into account additive velocity
                    if (_internalVelocityAdd.sqrMagnitude > 0f) {
                        currentVelocity += _internalVelocityAdd;
                        _internalVelocityAdd = Vector3.zero;
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///     (Called by KinematicCharacterMotor during its update cycle)
        ///     This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime) {
            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    break;
                }
            }
        }

        public void PostGroundingUpdate(float deltaTime) {
            // Handle landing and leaving ground
            if (Motor.GroundingStatus.IsStableOnGround && !Motor.LastGroundingStatus.IsStableOnGround)
                OnLanded();
            else if (!Motor.GroundingStatus.IsStableOnGround && Motor.LastGroundingStatus.IsStableOnGround) OnLeaveStableGround();
        }

        public bool IsColliderValidForCollisions(Collider coll) {
            if (IgnoredColliders.Count == 0) return true;

            if (IgnoredColliders.Contains(coll)) return false;

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) { }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport) { }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport) { }

        public void OnDiscreteCollisionDetected(Collider hitCollider) { }

        /// <summary>
        ///     Handles movement state transitions and enter/exit callbacks
        /// </summary>
        public void TransitionToState(CharacterState newState) {
            var tmpInitialState = CurrentCharacterState;
            OnStateExit(tmpInitialState, newState);
            CurrentCharacterState = newState;
            OnStateEnter(newState, tmpInitialState);
        }

        /// <summary>
        ///     Event when entering a state
        /// </summary>
        public void OnStateEnter(CharacterState state, CharacterState fromState) {
            switch (state) {
                case CharacterState.Default: {
                    break;
                }
            }
        }

        /// <summary>
        ///     Event when exiting a state
        /// </summary>
        public void OnStateExit(CharacterState state, CharacterState toState) {
            switch (state) {
                case CharacterState.Default: {
                    break;
                }
            }
        }

        /// <summary>
        ///     This is called every frame by ExamplePlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs) {
            // Clamp input
            var moveInputVector =
                Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            var cameraPlanarDirection =
                Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
                cameraPlanarDirection =
                    Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;

            var cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    // Move and look inputs
                    _moveInputVector = cameraPlanarRotation * moveInputVector;

                    switch (OrientationMethod) {
                        case OrientationMethod.TowardsCamera:
                            _lookInputVector = cameraPlanarDirection;
                            break;
                        case OrientationMethod.TowardsMovement:
                            _lookInputVector = _moveInputVector.normalized;
                            break;
                    }

                    break;
                }
            }
        }

        /// <summary>
        ///     This is called every frame by the AI script in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref AICharacterInputs inputs) {
            _moveInputVector = inputs.MoveVector;
            _lookInputVector = inputs.LookVector;
        }

        public void AddVelocity(Vector3 velocity) {
            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    _internalVelocityAdd += velocity;
                    break;
                }
            }
        }

        protected void OnLanded() { }

        protected void OnLeaveStableGround() { }
    }
}

public enum CharacterState {
    Default
}

public enum OrientationMethod {
    TowardsCamera,
    TowardsMovement
}

public struct PlayerCharacterInputs {
    public float MoveAxisForward;
    public float MoveAxisRight;
    public Quaternion CameraRotation;
    public bool JumpDown;
    public bool CrouchDown;
    public bool CrouchUp;
}

public struct AICharacterInputs {
    public Vector3 MoveVector;
    public Vector3 LookVector;
}

public enum BonusOrientationMethod {
    None,
    TowardsGravity,
    TowardsGroundSlopeAndGravity
}