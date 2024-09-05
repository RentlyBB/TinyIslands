using EditorScripts.InvokeButton;
using KinematicCharacterController;
using UnityEngine;
using World.AbstractClasses;

namespace World {
    [RequireComponent(typeof(PhysicsMover))]
    public class MovingPlatform : Interactable, IMoverController {
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
        
        [SerializeField]
        private bool _canMove = true;

        private void Awake() {
            TryGetComponent<PhysicsMover>(out _mover);
        }

        void Start() {
            _mover.MoverController = this;

            _goalPosition = pointA.position;
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
                ChangeDirectionMidJourney();
            } else {
                StartMovement();
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
                // Stop the platform
                _canMove = false;

                // Platform will wait here until Interact() is called again
            }
        }

        // Optional: If you want to change direction mid-movement, you can call this method anytime
        private void ChangeDirectionMidJourney() {
            if (_canMove) {
                // Immediately swap direction, preserving the current position as the new start point
                startPosition = transform.position;

                // Switch the target position
                if (movingToB) {
                    targetPosition = pointA.position;
                } else {
                    targetPosition = pointB.position;
                }

                movingToB = !movingToB;

                // Recalculate the journey length and reset the timer
                journeyLength = Vector3.Distance(startPosition, targetPosition);
                startTime = Time.time;
            }
        }

        private void StartMovement() {
            // Toggle the direction, and recalculate the journey
            if (movingToB) {
                // Switch to move towards pointA
                startPosition = pointB.position;
                targetPosition = pointA.position;
            } else {
                // Switch to move towards pointB
                startPosition = pointA.position;
                targetPosition = pointB.position;
            }

            movingToB = !movingToB;

            // Reset the journey properties
            journeyLength = Vector3.Distance(startPosition, targetPosition);
            startTime = Time.time;

            // Start moving the platform
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