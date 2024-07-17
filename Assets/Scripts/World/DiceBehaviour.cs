using System;
using EditorScripts;
using UnityEngine;

namespace World {
    public enum DiceFaces {
        Yellow,
        Red,
        Cyan,
        Green,
        Blue,
        Pink,
    }

    public class DiceBehaviour : MonoBehaviour {

        public DiceFaces currentSide;
        public Transform meshToRotate;
        public float moveSpeed = 10;
        public float rotSpeed = 10f;
        public float rotLerpSpeed = 0.5f;
        public float moveLerpSpeed = 0.5f;

        public bool rotationCompleted = true;

        [HideInInspector] public bool locked;
        [HideInInspector] public bool lockAnim;

        private Animator _animator;
        private Vector3 _currentPosition;
        private Quaternion _currentRotation;


        private readonly Quaternion[] _faceRotation = {
            Quaternion.Euler(0f, 0f, 0f), //Top
            Quaternion.Euler(180f, 0f, 0f), // Bottom
            Quaternion.Euler(-90f, 0f, 0f), // Front
            Quaternion.Euler(90f, 0f, 0f), // Back
            Quaternion.Euler(0f, 0f, -90f), // Left
            Quaternion.Euler(0f, 0f, 90f), // Right
        };

        private Vector3 _targetPosition;

        private Quaternion _targetRotation;

        private void Awake() {
            _currentPosition = transform.position;
            _targetPosition = transform.position;
        }

        private void Start() {
            _animator = GetComponent<Animator>();
            DisableAnimator();
            RotationToCurrent();
        }

        private void Update() {
            Moving();
            if (!rotationCompleted) {
                DisableAnimator();
                Rotating();
            } else {
                EnableAnimator();
            }
        }

        public void RotationToCurrent() {
            if (locked) return;

            RotateByIndex((int)currentSide);
        }


        // Rotate to the next face in the row
        [InvokeButton]
        public void RotateNext() {
            if (locked) return;

            if ((int)currentSide == Enum.GetValues(typeof(DiceFaces)).Length - 1) {
                currentSide = 0;
            } else {
                currentSide += 1;
            }
            RotationToCurrent();
        }

        //Set targetRotation
        private void RotateByIndex(int index) {
            if (locked) return;

            rotationCompleted = false;
            _targetRotation = _faceRotation[index];
        }

        public void SetTargetPosition(Vector3 targetPosition) {
            _targetPosition = targetPosition;
        }


        //TRANSFORM
        private void Moving() {

            if (Vector3.Distance(_currentPosition, _targetPosition) < 0.001f) return;

            Vector3 newPosition = Vector3.Slerp(_currentPosition, _targetPosition, moveLerpSpeed * Time.deltaTime * moveSpeed);

            transform.position = newPosition;

            _currentPosition = newPosition;
        }

        private void Rotating() {

            if (Quaternion.Angle(meshToRotate.rotation, _targetRotation) <= 0.01f) {
                rotationCompleted = true;
            }

            // Calculate the rotation towards the target using slerp
            Quaternion newRotation = Quaternion.Slerp(_currentRotation, _targetRotation, rotLerpSpeed * Time.deltaTime * rotSpeed);

            // Update the object's rotation
            meshToRotate.rotation = newRotation;

            // Update currentRotation for the next frame
            _currentRotation = newRotation;
        }


        //ANIMATION 
        [InvokeButton]
        public void Shake() {
            _animator.SetTrigger("ShakeIt");
        }

        private void DisableAnimator() {
            _animator.enabled = false;
        }

        private void EnableAnimator() {
            if (lockAnim) return;

            _animator.enabled = true;
        }
    }
}