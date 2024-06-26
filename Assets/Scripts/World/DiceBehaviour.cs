using System;
using System.Collections;
using System.Collections.Generic;
using EditorScripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace World {
    public enum DiceFaces {
        Yellow,
        Red,
        Cyan,
        Green,
        Blue,
        Pink
    }

    public class DiceBehaviour : MonoBehaviour {

        public DiceFaces currentSide;
        public float moveSpeed = 10;
        public float rotSpeed = 10f;
        public float lerpSpeed = 0.5f;

        public bool rotationCompleted = true;

        private Quaternion _targetRotation;
        private Quaternion _currentRotation;

        private Vector3 _targetPosition;
        private Vector3 _currentPosition;

        public bool locked = false;


        private Quaternion[] _faceRotation = new Quaternion[] {
            Quaternion.Euler(0f, 0f, 0f), //Top
            Quaternion.Euler(180f, 0f, 0f), // Bottom
            Quaternion.Euler(-90f, 0f, 0f), // Front
            Quaternion.Euler(90f, 0f, 0f), // Back
            Quaternion.Euler(0f, 0f, -90f), // Left
            Quaternion.Euler(0f, 0f, 90f) // Right
        };

        private void Awake() {
            _targetPosition = transform.position;
        }

        private void Start() {
            RotationToCurrent();
        }

        private void Update() {

            if (Quaternion.Angle(transform.rotation, _targetRotation) <= 0.01f) {
                rotationCompleted = true;
            } else {
                rotationCompleted = false;
            }

            Rotating();
            Moving();
        }
        

        [InvokeButton]
        public void RotationToCurrent() {
            if(locked) return;
            RotateByIndex((int)currentSide);
        }

        [InvokeButton]
        public void RandomRotate() {
            if(locked) return;
            DiceFaces rndFace;
            do {
                rndFace = (DiceFaces)Random.Range(0, 6);
            } while (rndFace == currentSide);

            currentSide = rndFace;

            RotationToCurrent();
        }

        [InvokeButton]
        public void RotateNext() {
            if(locked) return;
            if ((int)currentSide == Enum.GetValues(typeof(DiceFaces)).Length - 1) {
                currentSide = 0;
            } else {
                currentSide += 1;
            }
            RotationToCurrent();
        }


        private void RotateByIndex(int index) {
            if(locked) return;
            _targetRotation = _faceRotation[index];
        }

        public void SetTargetPosition(Vector3 targetPosition) {
            _targetPosition = targetPosition;
        }

        private void Moving() {
            Vector3 newPosition = Vector3.Slerp(_currentPosition, _targetPosition, lerpSpeed * Time.deltaTime * moveSpeed);

            transform.position = newPosition;
            
            _currentPosition = newPosition;
        }
        
        private void Rotating() {

            // Calculate the rotation towards the target using slerp
            Quaternion newRotation = Quaternion.Slerp(_currentRotation, _targetRotation, lerpSpeed * Time.deltaTime * rotSpeed);

            // Update the object's rotation
            transform.rotation = newRotation;

            // Update currentRotation for the next frame
            _currentRotation = newRotation;
        }
    }
}