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
                float currentHeight = hit.distance;
                float heightError = hoverHeight - currentHeight;
                float upwardSpeed = rb.velocity.y;
                float lift = heightError * hoverForce - upwardSpeed * hoverDamping;

                rb.AddForce(Vector3.up * lift, ForceMode.Acceleration);
            }
        }

        private void HandleMovement() {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            rb.AddForce(movement * moveSpeed, ForceMode.Acceleration);
        }
    }
}