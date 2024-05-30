using System;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Serialization;

namespace World {
    public class ElevatorBehaviour : MonoBehaviour, IMoverController {

        public PhysicsMover mover;
        public Vector3 offset;
        public float speed = 0.1f;
        public bool stayAtLastPosition = false;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private Vector3 _nextPosition;

        private Vector3 _currentPosition;
        private Vector3 _elevatorEndPosition;
        private bool _onOriginalPosition;

        private float _fraction;

        private void Start() {
            _originalPosition = mover.Rigidbody.position;
            _originalRotation = mover.Rigidbody.rotation;
            _nextPosition = _originalPosition + offset;
            _currentPosition = _originalPosition;
            _elevatorEndPosition = _originalPosition;
            _onOriginalPosition = true;

            mover.MoverController = this;


        }

        private void Update() {
            _nextPosition = _originalPosition + offset;
            
            if (_fraction < 1) {
                _fraction += Time.deltaTime * speed;
                _currentPosition = Vector3.Lerp(_currentPosition, _elevatorEndPosition, _fraction);
            }

            if (_currentPosition == _originalPosition) {
                _onOriginalPosition = true;
            } else if (_currentPosition == _originalPosition + offset) {
                _onOriginalPosition = false;
            }

        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = _currentPosition;
            goalRotation = _originalRotation;
        }

        private void ElevateTo(Vector3 elevateTarget) {
            _fraction = 0;
            _elevatorEndPosition = elevateTarget;
        }

        private void OnCollisionEnter(Collision other) {
            if (other.transform.CompareTag("Player")) {
                Vector3 elevateTo = _nextPosition;

                if (stayAtLastPosition) {
                    if (!_onOriginalPosition) {
                        elevateTo = _originalPosition;
                    } 
                }

                ElevateTo(elevateTo);
            }
        }

        private void OnCollisionExit(Collision other) {
            if (other.transform.CompareTag("Player")) {
                if (!stayAtLastPosition) {
                    ElevateTo(_originalPosition);
                } 
            }
        }
    }
}