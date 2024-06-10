using System;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Serialization;

namespace World {
    public class ElevatorBehaviour : MonoBehaviour, IMoverController {
        public PhysicsMover mover;
        public Vector3 nextPositon;

        [Range(0.1f, 1)] public float speed = 0.1f;
        public bool stayAtLastPosition = false;


        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        
        private Vector3 _currentGoalPosition;
        private Vector3 _nextGoalPosition;
        
        private bool _toOriginalPosition;


        private float _fraction;

        private void Start() {
            _originalPosition = mover.Rigidbody.position;
            _originalRotation = mover.Rigidbody.rotation;
            
            _currentGoalPosition = _originalPosition;
            _nextGoalPosition = _originalPosition;

            _toOriginalPosition = true;
            
            mover.MoverController = this;
        }

        void OnDrawGizmosSelected() {
            // Draw a yellow cube at the transform position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + nextPositon, new Vector3(4f, 0.2f, 4f));
        }

        private void Update() {

            if (_fraction < 1) {
                _fraction += Time.deltaTime * (speed / 10);
                _currentGoalPosition = Vector3.Lerp(_currentGoalPosition, _nextGoalPosition, _fraction);
            }
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = _currentGoalPosition;
            goalRotation = _originalRotation;
        }

        public void ElevateOnEvent(Interactable interactable) {
            if (!stayAtLastPosition) {
                Debug.LogWarning("To move the Elevator please set 'stayAtLastPosition' to true");
                return;
            }

            SelectNextPosition();
            
            //TODO: this has to be invoke when the action is done, not when the action is triggered
            interactable.ReadyToInteract();
        }


        private void SelectNextPosition() {
            
            Vector3 elevateTo = _originalPosition;

            if (stayAtLastPosition) {
                if (_toOriginalPosition) {
                    elevateTo = _originalPosition + nextPositon;
                    _toOriginalPosition = false;
                } else {
                    elevateTo = _originalPosition;
                    _toOriginalPosition = true;
                }
            }

            ElevateTo(elevateTo);
        }
        
        private void ElevateTo(Vector3 elevateTarget) {
            _fraction = 0;
            _nextGoalPosition = elevateTarget;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform.CompareTag("Player")) {
              SelectNextPosition();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.transform.CompareTag("Player")) {
                if (!stayAtLastPosition) {
                    ElevateTo(_originalPosition);
                }
            }
        }
    }
}