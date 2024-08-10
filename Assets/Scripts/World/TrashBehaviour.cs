using UnityEngine;

namespace World {
    public class TrashBehaviour : MonoBehaviour {
        public float speed = 10;

        public float distance;

        private Rigidbody _rb;
        private Vector3 _targetPosition;


        private void Awake() {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            // if (_targetPosition != Vector3.zero) {
            //     var dir = _targetPosition - transform.position;
            //
            //     _rb.AddForce(Time.fixedDeltaTime * speed * dir, ForceMode.Force);
            // }
        }

        public void TrashActivate(Vector3 targetPosition) {
            _targetPosition = targetPosition;
            distance = Vector3.Distance(transform.position, targetPosition);
            if (distance < 1f) Destroy(transform.gameObject);
        }
    }
}