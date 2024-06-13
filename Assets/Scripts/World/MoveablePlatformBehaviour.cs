using System;
using EditorScripts;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Serialization;

namespace World {
    [RequireComponent(typeof(PhysicsMover))]
    public class MoveablePlatformBehaviour : MonoBehaviour, IMoverController {
        public PhysicsMover mover;

        public Vector3 nextPositon;
        
        [Range(0.1f, 1)]
        public float speed = 0.1f;
        
        public bool loop = false;
        
        public Transform wireCubeSize;
        
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
            
            if (wireCubeSize == null) {
                wireCubeSize = transform;
            }
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + nextPositon, wireCubeSize.localScale);
            
        }

        private void Update() {
            if (_fraction < 1) {
                _fraction += Time.deltaTime * (speed / 10);
                _currentGoalPosition = Vector3.Lerp(_currentGoalPosition, _nextGoalPosition, _fraction);
            }

            LoopingMovement();


        }


        private void LoopingMovement() {
            if (!loop) return;

            if (transform.position == _nextGoalPosition) {
                MovePlatform();
            }

        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = _currentGoalPosition;
            goalRotation = _originalRotation;
        }

        [InvokeButton]
        public void MovePlatform() {
            SelectNextPosition();
        }


        private void SelectNextPosition() {
            Vector3 elevateTo;

            if (_toOriginalPosition) {
                elevateTo = _originalPosition + nextPositon;
                _toOriginalPosition = false;
            } else {
                elevateTo = _originalPosition;
                _toOriginalPosition = true;
            }

            ElevateTo(elevateTo);
        }

        private void ElevateTo(Vector3 elevateTarget) {
            _fraction = 0;
            _nextGoalPosition = elevateTarget;
        }
    }
}