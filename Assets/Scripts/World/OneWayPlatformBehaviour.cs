using System;
using System.Collections.Generic;
using EditorScripts.InvokeButton;
using KinematicCharacterController;
using UnityEngine;
using World.AbstractClasses;
using World.Enums;

namespace World {
    [RequireComponent(typeof(PhysicsMover))]
    [Obsolete("Use MovingPlatformInstead")]
    public class OneWayPlatformBehaviour : Interactable, IMoverController {
        
        private PhysicsMover _mover;

        public Transform target; // The target position the platform should move to

        public float speed = 5f; // Desired constant speed in units per second
        
        public bool startOnCurrentPosition = true; // At the start stay or move on the next position
        
        private Vector3 _startPosition; // Starting point
        private Vector3 _targetPosition; // Ending point
       
        
        
        private Vector3 _currentTargetPosition;
        private float _journeyLength;
        private float _timeStartedLerping;
        private bool _isMoving = false;

        private Vector3 _goalPosition;

        private Vector3 _velocity = Vector3.zero;

        private void Awake() {
            TryGetComponent<PhysicsMover>(out _mover);
        }

        private void Start() {
            _mover.MoverController = this;

            _startPosition = transform.position; // Save the initial position
            _targetPosition = target.position;
            
            _currentTargetPosition = _startPosition; // Start with the assigned target position

            _goalPosition = transform.position;

            if (!startOnCurrentPosition) {
                Interact();
            }
        }

        private void FixedUpdate() {
            Movement();
        }

        private void Movement2() {
            if (_isMoving) {
                float distanceCovered = (Time.time - _timeStartedLerping) * speed;
                float fractionOfJourney = distanceCovered / _journeyLength;

                // Apply easing function to fraction for smooth start and end
                float easedFraction = EaseInOutQuad(fractionOfJourney);

                // Move the platform towards the target at a constant speed with easing
                _goalPosition = Vector3.Lerp(_startPosition, _currentTargetPosition, easedFraction);
                

                // Check if the platform has reached the target position
                if (fractionOfJourney >= 1f) {
                    // Stop moving and wait for a new target
                    _isMoving = false;
                }
            }
        }
        
        private void Movement() {
           
                // Move the platform towards the target at a constant speed with easing
                _goalPosition = Vector3.SmoothDamp(transform.position, _currentTargetPosition, ref _velocity, speed * Time.fixedDeltaTime);
               
        }

        private void StartMovement() {
            _startPosition = transform.position;
            _journeyLength = Vector3.Distance(_startPosition, _currentTargetPosition);
            _timeStartedLerping = Time.time;
            _isMoving = true; // Start moving towards the current target
        }

        public void ToggleTargetPosition() {
            // Toggle between the target position and the original start position
            _currentTargetPosition = _currentTargetPosition == _targetPosition ? _startPosition : _targetPosition;
        }
        
        [InvokeButton]
        public override void Interact() {
            ToggleTargetPosition();
            StartMovement();
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = _goalPosition;
            goalRotation = _mover.Rigidbody.rotation;
        }
        
        // Easing function for smooth start and end (Ease In Out Quadratic)
        private float EaseInOutQuad(float t) {
            return t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
        }
        
    }
}