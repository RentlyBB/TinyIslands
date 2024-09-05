using System;
using EditorScripts.InvokeButton;
using KinematicCharacterController;
using UnityEngine;
using World.AbstractClasses;

namespace World {
    [RequireComponent(typeof(PhysicsMover))]
    public class AutomaticMovingPlatformBehaviour : Interactable, IMoverController {
        public Transform pointA;
        public Transform pointB;
        public float speed = 2f;

        private PhysicsMover _mover;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Vector3 _goalPosition;
        private bool movingToB = true;
        private float journeyLength;
        private float startTime;

        private bool _canMove = true;
        private float _pauseTime;

        private void Awake() {
            TryGetComponent<PhysicsMover>(out _mover);
        }

        void Start() {
            _mover.MoverController = this;

            _goalPosition = transform.position;
            startPosition = pointA.position;
            targetPosition = pointB.position;
            journeyLength = Vector3.Distance(startPosition, targetPosition);
            startTime = Time.time;


            pointA.SetParent(null);
            pointB.SetParent(null);
        }

        [InvokeButton]
        public override void Interact() {
            if (_canMove) {
                PausePlatform();
            } else {
                ResumePlatform();
            }
        }

        [InvokeButton]
        public void ResetPoints() {
            pointA.position = transform.position;
            pointB.position = transform.position;
        }

        void Update() {
            if (_canMove) {
                MovePlatformSmoothly();
            }
        }

        private void MovePlatformSmoothly() {
            // Calculate the fraction of the journey completed
            float distCovered = (Time.time - startTime) * speed;
            float fractionOfJourney = distCovered / journeyLength;

            // Smooth step for easing movement
            float smoothStep = Mathf.SmoothStep(0, 1, fractionOfJourney);

            // Move the platform
            _goalPosition = Vector3.Lerp(startPosition, targetPosition, smoothStep);

            // Check if the platform has reached the target position
            if (fractionOfJourney >= 1f) {
                if (movingToB) {
                    // Switch to moving to pointA
                    var tempPos = targetPosition;
                    targetPosition = startPosition;
                    startPosition = tempPos;

                } else {
                    // Switch to moving to pointB
                    var tempPos = startPosition;
                    startPosition = targetPosition;
                    targetPosition = tempPos;
                }

                movingToB = !movingToB;

                // Reset time and distance for the new journey
                journeyLength = Vector3.Distance(startPosition, targetPosition);
                startTime = Time.time;
            }
        }

        // Method to pause the platform
        public void PausePlatform() {
            _pauseTime = Time.time; // Record the time when paused
            _canMove = false;
        }

        // Method to resume the platform
        public void ResumePlatform() {
            // Adjust startTime to account for time spent paused
            startTime += Time.time - _pauseTime;
            _canMove = true;
        }

        // Draw gizmos to visualize points and lines between them
        void OnDrawGizmos() {
            // Only draw if both points are assigned
            if (pointA != null && pointB != null) {
                // Draw a line between point A and point B
                Gizmos.color = Color.green;
                Gizmos.DrawLine(pointA.position, pointB.position);

                // Draw small spheres at point A and point B to mark them
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(pointA.position, 0.5f);
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(pointB.position, 0.5f);
            }
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = _goalPosition;
            goalRotation = _mover.Rigidbody.rotation;
        }
    }
}