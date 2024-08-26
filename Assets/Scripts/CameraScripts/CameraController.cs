using System;
using EditorScripts.InvokeButton;
using InputCore;
using PlayerCharacter;
using UnityEngine;

namespace CameraScripts {
    public class CameraController : MonoBehaviour {
        public InputReaderSo inputReader;

        [Space]
        [Header("Zoom Settings")]
        public float minScrollZoomSize = 1.0f; // Minimum orthographic size (zoom out limit)

        public float maxScrollZoomSize = 50.0f; // Maximum orthographic size (zoom in limit)
        public bool canScrollZooming;

        [Space]
        public float zoomSmoothing = 5.0f; // Smoothing factor for zooming

        public float zoomSpeed = 25.0f; // Speed of zooming

        [Space]
        public float zoomInSize = 10f;

        public float zoomOutSize = 50.0f;

        [Space]
        [Header("Camera Movement Settings")]
        public float moveSmoothTime = 0.3f; // Time for the smooth 

        public float zOffset = -100;

        private Camera _camera;

        private Vector3 _lastCalculatedTargetPosition; // WARNING! This is a position with calculated offset, do not put this into SetCameraTargetPosition() method
        private Vector3 _targetPosition;
        private float _targetZoomSize = 50;

        private Vector3 _velocity = Vector3.zero;

        public Transform testingCamPos;

        private void Start() {
            _camera = GetComponent<CharacterCamera>().Camera;
            _targetZoomSize = zoomInSize;
            _camera.orthographicSize = _targetZoomSize;

            SetCameraTargetPosition(new Vector3());
            
        }

        [InvokeButton]
        public void ResetCamera() {
            var temp = testingCamPos;
            testingCamPos = null;
            testingCamPos = temp;
            transform.position =  testingCamPos.position + transform.TransformDirection(new Vector3(0, 0, zOffset));
        }

        private void LateUpdate() {
            ZoomingCamera();
            SetCameraTargetPosition(testingCamPos.position);
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, moveSmoothTime);
        }

        private void OnValidate() {
            if (testingCamPos != null) {
                transform.position =  testingCamPos.position + transform.TransformDirection(new Vector3(0, 0, zOffset));
            }
        }

        private void OnEnable() {
            inputReader.ZoomOut += ZoomCameraOut;
            inputReader.ZoomIn += ZoomCameraIn;
        }

        private void OnDisable() {
            inputReader.ZoomOut -= ZoomCameraOut;
            inputReader.ZoomIn -= ZoomCameraIn;
        }


        private void ZoomingCamera() {
            if (canScrollZooming) {
                var zoomInput = Input.GetAxis("Mouse ScrollWheel");
                if (zoomInput != 0)
                    // Calculate the target orthographic size based on the zoom input
                    _targetZoomSize = Mathf.Clamp(_targetZoomSize - zoomInput * zoomSpeed, minScrollZoomSize, maxScrollZoomSize);
            }

            // Smoothly interpolate towards the target orthographic size
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoomSize, zoomSmoothing * Time.deltaTime);
        }

        private void ZoomCameraOut() {
            _targetZoomSize = zoomOutSize;

            // Save last target position with calculated camera offset;
            _lastCalculatedTargetPosition = _targetPosition;

            SetCameraTargetPosition(new Vector3(-4.71f, 1.75f, -4.57f));
        }

        private void ZoomCameraIn() {
            _targetZoomSize = zoomInSize;
            SetCalculatedCameraTargetPosition(_lastCalculatedTargetPosition);
        }


        // Set position with already calculated offset
        public void SetCalculatedCameraTargetPosition(Vector3 targetPos) {
            _targetPosition = targetPos;
        }


        // Set position and add a offset to the camera
        public void SetCameraTargetPosition(Vector3 targetPos) {
            _targetPosition = targetPos + transform.TransformDirection(new Vector3(0, 0, zOffset));
        }
    }
}