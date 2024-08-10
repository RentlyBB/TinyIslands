using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController.Walkthrough.ClimbingLadders {
    public enum CharacterState {
        Default,
        Climbing
    }

    public enum ClimbingState {
        Anchoring,
        Climbing,
        DeAnchoring
    }

    public struct PlayerCharacterInputs {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool CrouchDown;
        public bool CrouchUp;
        public bool ClimbLadder;
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

        [Header("Ladder Climbing")]
        public float ClimbingSpeed = 4f;

        public float AnchoringDuration = 0.25f;
        public LayerMask InteractionLayer;

        [Header("Misc")]
        public List<Collider> IgnoredColliders = new();

        public bool OrientTowardsGravity;
        public Vector3 Gravity = new(0, -30f, 0);
        public Transform MeshRoot;

        private readonly Collider[] _probedColliders = new Collider[8];
        private Vector3 _anchoringStartPosition = Vector3.zero;
        private Quaternion _anchoringStartRotation = Quaternion.identity;
        private float _anchoringTimer;
        private bool _canWallJump;
        private bool _doubleJumpConsumed;
        private ClimbingState _internalClimbingState;
        private Vector3 _internalVelocityAdd = Vector3.zero;
        private bool _isCrouching;
        private bool _jumpConsumed;
        private bool _jumpedThisFrame;
        private bool _jumpRequested;
        private Vector3 _ladderTargetPosition;
        private Quaternion _ladderTargetRotation;

        // Ladder vars
        private float _ladderUpDownInput;
        private Vector3 _lookInputVector;
        private Vector3 _moveInputVector;
        private float _onLadderSegmentState;
        private Quaternion _rotationBeforeClimbing = Quaternion.identity;
        private bool _shouldBeCrouching;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump;
        private Vector3 _wallJumpNormal;

        public CharacterState CurrentCharacterState { get; private set; }
        private MyLadder _activeLadder { get; set; }

        private ClimbingState _climbingState {
            get => _internalClimbingState;
            set {
                _internalClimbingState = value;
                _anchoringTimer = 0f;
                _anchoringStartPosition = Motor.TransientPosition;
                _anchoringStartRotation = Motor.TransientRotation;
            }
        }

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
        public void BeforeCharacterUpdate(float deltaTime) { }

        /// <summary>
        ///     (Called by KinematicCharacterMotor during its update cycle)
        ///     This is where you tell your character what its rotation should be right now.
        ///     This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime) {
            switch (CurrentCharacterState) {
                case CharacterState.Default: {
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
                case CharacterState.Climbing: {
                    switch (_climbingState) {
                        case ClimbingState.Climbing:
                            currentRotation = _activeLadder.transform.rotation;
                            break;
                        case ClimbingState.Anchoring:
                        case ClimbingState.DeAnchoring:
                            currentRotation = Quaternion.Slerp(_anchoringStartRotation, _ladderTargetRotation, _anchoringTimer / AnchoringDuration);
                            break;
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
                case CharacterState.Climbing: {
                    currentVelocity = Vector3.zero;

                    switch (_climbingState) {
                        case ClimbingState.Climbing:
                            currentVelocity = (_ladderUpDownInput * _activeLadder.transform.up).normalized * ClimbingSpeed;
                            break;
                        case ClimbingState.Anchoring:
                        case ClimbingState.DeAnchoring:
                            var tmpPosition = Vector3.Lerp(_anchoringStartPosition, _ladderTargetPosition, _anchoringTimer / AnchoringDuration);
                            currentVelocity = Motor.GetVelocityForMovePosition(Motor.TransientPosition, tmpPosition, deltaTime);
                            break;
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
                case CharacterState.Climbing: {
                    switch (_climbingState) {
                        case ClimbingState.Climbing:
                            // Detect getting off ladder during climbing
                            _activeLadder.ClosestPointOnLadderSegment(Motor.TransientPosition, out _onLadderSegmentState);
                            if (Mathf.Abs(_onLadderSegmentState) > 0.05f) {
                                _climbingState = ClimbingState.DeAnchoring;

                                // If we're higher than the ladder top point
                                if (_onLadderSegmentState > 0) {
                                    _ladderTargetPosition = _activeLadder.TopReleasePoint.position;
                                    _ladderTargetRotation = _activeLadder.TopReleasePoint.rotation;
                                }
                                // If we're lower than the ladder bottom point
                                else if (_onLadderSegmentState < 0) {
                                    _ladderTargetPosition = _activeLadder.BottomReleasePoint.position;
                                    _ladderTargetRotation = _activeLadder.BottomReleasePoint.rotation;
                                }
                            }

                            break;
                        case ClimbingState.Anchoring:
                        case ClimbingState.DeAnchoring:
                            // Detect transitioning out from anchoring states
                            if (_anchoringTimer >= AnchoringDuration) {
                                if (_climbingState == ClimbingState.Anchoring)
                                    _climbingState = ClimbingState.Climbing;
                                else if (_climbingState == ClimbingState.DeAnchoring) TransitionToState(CharacterState.Default);
                            }

                            // Keep track of time since we started anchoring
                            _anchoringTimer += deltaTime;
                            break;
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
                    break;
                }
                case CharacterState.Climbing: {
                    _rotationBeforeClimbing = Motor.TransientRotation;

                    Motor.SetMovementCollisionsSolvingActivation(false);
                    Motor.SetGroundSolvingActivation(false);
                    _climbingState = ClimbingState.Anchoring;

                    // Store the target position and rotation to snap to
                    _ladderTargetPosition = _activeLadder.ClosestPointOnLadderSegment(Motor.TransientPosition, out _onLadderSegmentState);
                    _ladderTargetRotation = _activeLadder.transform.rotation;
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
                case CharacterState.Climbing: {
                    Motor.SetMovementCollisionsSolvingActivation(true);
                    Motor.SetGroundSolvingActivation(true);
                    break;
                }
            }
        }

        /// <summary>
        ///     This is called every frame by MyPlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs) {
            // Handle ladder transitions
            _ladderUpDownInput = inputs.MoveAxisForward;
            if (inputs.ClimbLadder)
                if (Motor.CharacterOverlap(Motor.TransientPosition, Motor.TransientRotation, _probedColliders, InteractionLayer, QueryTriggerInteraction.Collide) > 0)
                    if (_probedColliders[0] != null) {
                        // Handle ladders
                        var ladder = _probedColliders[0].gameObject.GetComponent<MyLadder>();
                        if (ladder) {
                            // Transition to ladder climbing state
                            if (CurrentCharacterState == CharacterState.Default) {
                                _activeLadder = ladder;
                                TransitionToState(CharacterState.Climbing);
                            }
                            // Transition back to default movement state
                            else if (CurrentCharacterState == CharacterState.Climbing) {
                                _climbingState = ClimbingState.DeAnchoring;
                                _ladderTargetPosition = Motor.TransientPosition;
                                _ladderTargetRotation = _rotationBeforeClimbing;
                            }
                        }
                    }

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