using System;
using System.Collections;
using System.Collections.Generic;
using EditorScripts;
using Unity.VisualScripting;
using UnityEngine;

namespace World {

    public enum DiceFaces {
        Top,
        Bottom,
        Front,
        Back,
        Left,
        Right
    }

    public class DiceBehaviour : MonoBehaviour {

        public DiceFaces currentSide;

        private Quaternion [] _faceRotation = new Quaternion [] {
            Quaternion.Euler (0f, 0f, 0f), //Top
            Quaternion.Euler(180f, 0f, 0f), // Bottom
            Quaternion.Euler(-90f, 0f, 0f), // Front
            Quaternion.Euler(90f, 0f, 0f), // Back
            Quaternion.Euler(0f, 0f, -90f), // Left
            Quaternion.Euler(0f, 0f, 90f) // Right
        };

        private Quaternion _targetRotation;
        private Quaternion _currentRotation;
        public float rotSpeed = 90f;
        public float lerpSpeed = 0.5f;


        private void Start() {
            SelectRotation();
        }

        [InvokeButton]
        public void SelectRotation() {
            switch (currentSide) {
                case DiceFaces.Top:
                    RotateToFace(0);
                    return;
                case DiceFaces.Bottom:
                    RotateToFace(1);
                    return;
                case DiceFaces.Front:
                    RotateToFace(2);
                    return;
                case DiceFaces.Back:
                    RotateToFace(3);
                    return;
                case DiceFaces.Left:
                    RotateToFace(4);
                    return;
                case DiceFaces.Right:
                    RotateToFace(5);
                    return;
            }
        }

        private void Update() {
            // Calculate the rotation towards the target using slerp
            Quaternion newRotation = Quaternion.Slerp(_currentRotation, _targetRotation, lerpSpeed * Time.deltaTime * rotSpeed);

            // Update the object's rotation
            transform.rotation = newRotation;

            // Update currentRotation for the next frame
            _currentRotation = newRotation;
        }


        private void RotateToFace(int index) {
            
            _targetRotation = _faceRotation[index];
        }
    }
}