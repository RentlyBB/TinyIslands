using UnityEngine;

namespace HoveringMovement {
    public class Hovering : MonoBehaviour {
        public float hoverHeight = 2.0f; // The height at which the robot should hover
        public float hoverForce = 5.0f; // The force applied to maintain the hover height
        public float hoverDamping = 0.5f; // The damping to stabilize the hover movement
        public float moveSpeed = 5.0f; // Speed of horizontal movement

        private Rigidbody rb;

        private void Start() {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            MaintainHoverHeight();
            HandleMovement();
        }

        private void MaintainHoverHeight() {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit)) {
                var currentHeight = hit.distance;
                var heightError = hoverHeight - currentHeight;
                var upwardSpeed = rb.velocity.y;
                var lift = heightError * hoverForce - upwardSpeed * hoverDamping;

                rb.AddForce(Vector3.up * lift, ForceMode.Acceleration);
            }
        }

        private void HandleMovement() {
            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Vertical");

            var movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * moveSpeed, ForceMode.Acceleration);
        }
    }
}