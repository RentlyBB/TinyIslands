using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController.Walkthrough.SwimmingState {
    public enum CharacterState {
        Default,
        Swimming
    }

    public struct PlayerCharacterInputs {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool JumpHeld;
        public bool CrouchDown;
        public bool CrouchUp;
        public bool CrouchHeld;
    }

    public class MyCharacterController : MonoBehaviour, ICharacterController {
        public KinematicCharacterMotor Motor;

        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;

        public float StableMovementSharpness = 15;
        public float OrientationSharpness = 10;
        public float MaxStableDistanceFromLedge = 5f;

        [Range(0f, 180f)]
        public float MaxStableDenivelationAngle = 180f;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 10f;

        public float AirAccelerationSpeed = 5f;
        public float Drag = 0.1f;

        [Header("Jumping")]
        public bool AllowJumpingWhenSliding;

        public bool AllowDoubleJump;
        public bool AllowWallJump;
        public float JumpSpeed = 10f;
        public float JumpPreGroundingGraceTime;
        public float JumpPostGroundingGraceTime;

        [Header("Swimming")]
        public Transform SwimmingReferencePoint;

        public LayerMask WaterLayer;
        public float SwimmingSpeed = 4f;
        public float SwimmingMovementSharpness = 3;
        public float SwimmingOrientationSharpness = 2f;

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new();

        public bool OrientTowardsGravity;
        public Vector3 Gravity = new(0, -30f, 0);
        public Transform MeshRoot;

        private readonly Collider[] _probedColliders = new Collider[8];
        private bool _canWallJump;
        private bool _crouchInputIsHeld;
        private bool _doubleJumpConsumed;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _isCrouching;
        private bool _jumpConsumed;
        private bool _jumpedThisFrame;
        private bool _jumpInputIsHeld;
        private bool _jumpRequested;
        private Vector3 _lookInputVector;
        private Vector3 _moveInputVector;
        private bool _shouldBeCrouching;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump;
        private Vector3 _wallJumpNormal;
        private Collider _waterZone;

        public CharacterState CurrentCharacterState { get; private set; }

        private void Start() {
            // Assign to motor
            Motor.CharacterController = this;

            // Handle initial state
            TransitionToState(CharacterState.Default);
        }

        /// <summary>
        ///     (Called by KinematicCharacterMotor during its update cycle)
        ///     This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime) {
            // Handle detecting water surfaces
            {
                // Do a character overlap test to detect water surfaces
                if (Motor.CharacterOverlap(Motor.TransientPosition, Motor.TransientRotation, _probedColliders, WaterLayer, QueryTriggerInteraction.Collide) > 0)
                    // If a water surface was detected
                    if (_probedColliders[0] != null) {
                        // If the swimming reference point is inside the box, make sure we are in swimming state
                        if (Physics.ClosestPoint(SwimmingReferencePoint.position, _probedColliders[0], _probedColliders[0].transform.position, _probedColliders[0].transform.rotation) ==
                            SwimmingReferencePoint.position) {
                            if (CurrentCharacterState == CharacterState.Default) {
                                TransitionToState(CharacterState.Swimming);
                                _waterZone = _probedColliders[0];
                            }
                        }
                        // otherwise; default state
                        else {
                            if (CurrentCharacterState == CharacterState.Swimming) TransitionToState(CharacterState.Default);
                        }
                    }
            }
        }

        /// <summary>
        ///     (Called by KinematicCharacterMotor during its update cycle)
        ///     This is where you tell your character what its rotation should be right now.
        ///     This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            switch (CurrentCharacterState) {
                case CharacterState.Default:
                case CharacterState.Swimming: {
                    if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f) {
                        // Smoothly interpolate from current to target look direction
                        var smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                        // Set the current rotation (which will be used by the KinematicCharacterMotor)
                        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                    }

                    if (OrientTowardsGravity)
                        // Rotate from current up to invert gravity
                        currentRotation = Quaternion.FromToRotation(currentRotation * Vector3.up, -Gravity) * currentRotation;
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
                    var targetMovementVelocity = Vector3.zero;
                    if (Motor.GroundingStatus.IsStableOnGround) {
                        // Reorient velocity on slope
                        currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                        // Calculate target velocity
                        var inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                        var reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                        targetMovementVelocity = reorientedInput * MaxStableMoveSpeed;

                        // Smooth movement Velocity
                        currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
                    } else {
                        // Add move input
                        if (_moveInputVector.sqrMagnitude > 0f) {
                            targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                            // Prevent climbing on un-stable slopes with air movement
                            if (Motor.GroundingStatus.FoundAnyGround) {
                                var perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                                targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpenticularObstructionNormal);
                            }

                            var velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                            currentVelocity += velocityDiff * AirAccelerationSpeed * deltaTime;
                        }

                        // Gravity
                        currentVelocity += Gravity * deltaTime;

                        // Drag
                        currentVelocity *= 1f / (1f + Drag * deltaTime);
                    }

                    // Handle jumping
                    {
                        _jumpedThisFrame = false;
                        _timeSinceJumpRequested += deltaTime;
                        if (_jumpRequested) {
                            // Handle double jump
                            if (AllowDoubleJump)
                                if (_jumpConsumed && !_doubleJumpConsumed && (AllowJumpingWhenSliding ? !Motor.GroundingStatus.FoundAnyGround : !Motor.GroundingStatus.IsStableOnGround)) {
                                    Motor.ForceUnground();

                                    // Add to the return velocity and reset jump state
                                    currentVelocity += Motor.CharacterUp * JumpSpeed - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                    _jumpRequested = false;
                                    _doubleJumpConsumed = true;
                                    _jumpedThisFrame = true;
                                }

                            // See if we actually are allowed to jump
                            if (_canWallJump ||
                                (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) ||
                                                    _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime))) {
                                // Calculate jump direction before ungrounding
                                var jumpDirection = Motor.CharacterUp;
                                if (_canWallJump)
                                    jumpDirection = _wallJumpNormal;
                                else if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround) jumpDirection = Motor.GroundingStatus.GroundNormal;

                                // Makes the character skip ground probing/snapping on its next update. 
                                // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                                Motor.ForceUnground();

                                // Add to the return velocity and reset jump state
                                currentVelocity += jumpDirection * JumpSpeed - Vector3.Project(currentVelocity, Motor.CharacterUp);
                                _jumpRequested = false;
                                _jumpConsumed = true;
                                _jumpedThisFrame = true;
                            }
                        }

                        // Reset wall jump
                        _canWallJump = false;
                    }

                    // Take into account additive velocity
                    if (_internalVelocityAdd.sqrMagnitude > 0f) {
                        currentVelocity += _internalVelocityAdd;
                        _internalVelocityAdd = Vector3.zero;
                    }

                    break;
                }
                case CharacterState.Swimming: {
                    var verticalInput = 0f + (_jumpInputIsHeld ? 1f : 0f) + (_crouchInputIsHeld ? -1f : 0f);

                    // Smoothly interpolate to target swimming velocity
                    var targetMovementVelocity = (_moveInputVector + Motor.CharacterUp * verticalInput).normalized * SwimmingSpeed;
                    var smoothedVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-SwimmingMovementSharpness * deltaTime));

                    // See if our swimming reference point would be out of water after the movement from our velocity has been applied
                    {
                        var resultingSwimmingReferancePosition = Motor.TransientPosition + smoothedVelocity * deltaTime + (SwimmingReferencePoint.position - Motor.TransientPosition);
                        var closestPointWaterSurface = Physics.ClosestPoint(resultingSwimmingReferancePosition, _waterZone, _waterZone.transform.position, _waterZone.transform.rotation);

                        // if our position would be outside the water surface on next update, project the velocity on the surface normal so that it would not take us out of the water
                        if (closestPointWaterSurface != resultingSwimmingReferancePosition) {
                            var waterSurfaceNormal = (resultingSwimmingReferancePosition - closestPointWaterSurface).normalized;
                            smoothedVelocity = Vector3.ProjectOnPlane(smoothedVelocity, waterSurfaceNormal);

                            // Jump out of water
                            if (_jumpRequested) smoothedVelocity += Motor.CharacterUp * JumpSpeed - Vector3.Project(currentVelocity, Motor.CharacterUp);
                        }
                    }

                    currentVelocity = smoothedVelocity;
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
                    // Handle jump-related values
                    {
                        // Handle jumping pre-ground grace period
                        if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime) _jumpRequested = false;

                        if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) {
                            // If we're on a ground surface, reset jumping values
                            if (!_jumpedThisFrame) {
                                _doubleJumpConsumed = false;
                                _jumpConsumed = false;
                            }

                            _timeSinceLastAbleToJump = 0f;
                        } else {
                            // Keep track of time since we were last able to jump (for grace period)
                            _timeSinceLastAbleToJump += deltaTime;
                        }
                    }

                    // Handle uncrouching
                    if (_isCrouching && !_shouldBeCrouching) {
                        // Do an overlap test with the character's standing height to see if there are any obstructions
                        Motor.SetCapsuleDimensions(0.5f, 2f, 1f);
                        if (Motor.CharacterOverlap(
                                Motor.TransientPosition,
                                Motor.TransientRotation,
                                _probedColliders,
                                Motor.CollidableLayers,
                                QueryTriggerInteraction.Ignore) > 0) {
                            // If obstructions, just stick to crouching dimensions
                            Motor.SetCapsuleDimensions(0.5f, 1f, 0.5f);
                        } else {
                            // If no obstructions, uncrouch
                            MeshRoot.localScale = new Vector3(1f, 1f, 1f);
                            _isCrouching = false;
                        }
                    }

                    break;
                }
            }
        }

        public bool IsColliderValidForCollisions(Collider coll) {
            if (IgnoredColliders.Contains(coll)) return false;
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) { }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport) {
            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    // We can wall jump only if we are not stable on ground and are moving against an obstruction
                    if (AllowWallJump && !Motor.GroundingStatus.IsStableOnGround && !hitStabilityReport.IsStable) {
                        _canWallJump = true;
                        _wallJumpNormal = hitNormal;
                    }

                    break;
                }
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation,
            ref HitStabilityReport hitStabilityReport) { }

        public void PostGroundingUpdate(float deltaTime) { }

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
                    Motor.SetGroundSolvingActivation(true);
                    break;
                }
                case CharacterState.Swimming: {
                    Motor.SetGroundSolvingActivation(false);
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
        ///     This is called every frame by MyPlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs) {
            _jumpInputIsHeld = inputs.JumpHeld;
            _crouchInputIsHeld = inputs.CrouchHeld;

            // Clamp input
            var moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            var cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f) cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            var cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    // Move and look inputs
                    _moveInputVector = cameraPlanarRotation * moveInputVector;
                    _lookInputVector = cameraPlanarDirection;

                    // Jumping input
                    if (inputs.JumpDown) {
                        _timeSinceJumpRequested = 0f;
                        _jumpRequested = true;
                    }

                    // Crouching input
                    if (inputs.CrouchDown) {
                        _shouldBeCrouching = true;

                        if (!_isCrouching) {
                            _isCrouching = true;
                            Motor.SetCapsuleDimensions(0.5f, 1f, 0.5f);
                            MeshRoot.localScale = new Vector3(1f, 0.5f, 1f);
                        }
                    } else if (inputs.CrouchUp) {
                        _shouldBeCrouching = false;
                    }

                    break;
                }
                case CharacterState.Swimming: {
                    _jumpRequested = inputs.JumpHeld;

                    _moveInputVector = inputs.CameraRotation * moveInputVector;
                    _lookInputVector = cameraPlanarDirection;
                    break;
                }
            }
        }

        public void AddVelocity(Vector3 velocity) {
            switch (CurrentCharacterState) {
                case CharacterState.Default: {
                    _internalVelocityAdd += velocity;
                    break;
                }
            }
        }
    }
}