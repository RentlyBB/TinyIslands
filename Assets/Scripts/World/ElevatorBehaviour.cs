using System;
using KinematicCharacterController;
using UnityEngine;

namespace World {
    public class ElevatorBehaviour : MonoBehaviour, IMoverController {

        public PhysicsMover Mover;
        public Vector3 offset;
        public float speed = 0.1f;
        public bool stayAtLastPosition = false;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        private Vector3 _currentPosition;
        private Vector3 _elevatorEndPoint;

        private float _fraction;

        private void Start() {
            _originalPosition = Mover.Rigidbody.position;
            _originalRotation = Mover.Rigidbody.rotation;
            _currentPosition = _originalPosition;
            _elevatorEndPoint = _originalPosition;


            Mover.MoverController = this;


        }

        private void Update() {
            
            if (_fraction < 1) {
                _fraction += Time.deltaTime * speed;
                _currentPosition = Vector3.Lerp(_currentPosition, _elevatorEndPoint, _fraction);
            }

        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = _currentPosition;
            goalRotation = _originalRotation;
        }

        private void ElevateTo(Vector3 elevateTarget) {
            _fraction = 0;
            _elevatorEndPoint = elevateTarget;
        }

        private void OnCollisionEnter(Collision other) {
            if (other.transform.CompareTag("Player")) {
                // if (stayAtLastPosition) {
                //     
                // } else {
                //     
                // }
                ElevateTo(_originalPosition + offset);
            }
        }

        private void OnCollisionExit(Collision other) {
            if (other.transform.CompareTag("Player")) {
                ElevateTo(_originalPosition);
            }
        }
    }
}