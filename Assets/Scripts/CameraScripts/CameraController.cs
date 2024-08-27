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
        public float defaultZoomInSize = 15f;
        public float defaultZoomOutSize = 50.0f;

        [Space]
        [Header("Camera Movement Settings")]
        public float moveSmoothTime = 0.3f; // Time for the smooth 

        public float zOffset = -100;

        private Camera _camera;

        private Vector3 _lastCalculatedTargetPosition; // WARNING! This is a position with calculated offset, do not put this into SetCameraTargetPosition() method
        private Vector3 _targetPosition;
        private float _targetZoomSize = 50;

        private Vector3 _velocity = Vector3.zero;
        private float _floatVelocity = 0f;
        
        
        private bool _zoomingEnabled = true;
        private bool _movingEnabled = true;


        // just for level design testing
        public Transform testingCamPos;

        private void Start() {
            _camera = GetComponent<CharacterCamera>().Camera;
            _targetZoomSize = defaultZoomInSize;
            _camera.orthographicSize = _targetZoomSize;

            SetCameraTargetPosition(new Vector3());
        }
        
        private void LateUpdate() {
            ScrollingZoom();
            ZoomingCamera();
            MovingCamera();
        }

        #if UNITY_EDITOR
        private void OnValidate() {
            if (testingCamPos != null) {
                transform.position =  testingCamPos.position + transform.TransformDirection(new Vector3(0, 0, zOffset));
            }
        }
        #endif

        private void OnEnable() {
            inputReader.ZoomOut += ZoomCameraOut;
            inputReader.ZoomIn += ZoomCameraIn;
        }

        private void OnDisable() {
            inputReader.ZoomOut -= ZoomCameraOut;
            inputReader.ZoomIn -= ZoomCameraIn;
        }

        private void ZoomingCamera() {
            

            // Smoothly interpolate towards the target orthographic size
            //_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoomSize, zoomSmoothing * (zoomSpeed/100) * Time.deltaTime);
            if (_zoomingEnabled) {
                _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, _targetZoomSize, ref _floatVelocity, zoomSmoothing);
            }
            
            //if near target, stop zooming
            if (Mathf.Abs(_targetZoomSize - _camera.orthographicSize) < 0.01) {
                _zoomingEnabled = false;
            }
        }

        public void ScrollingZoom() {
            if (canScrollZooming) {
                float zoomInput = Input.GetAxis("Mouse ScrollWheel");
                if (zoomInput != 0) {
                    // Calculate the target orthographic size based on the zoom input
                    _targetZoomSize = Mathf.Clamp(_targetZoomSize - zoomInput * zoomSpeed, minScrollZoomSize, maxScrollZoomSize);
                }
            }
        }

        private void ZoomCameraOut() {
            _targetZoomSize = defaultZoomOutSize;

            // Save last target position with calculated camera offset;
            _lastCalculatedTargetPosition = _targetPosition;

            // Center of the map - 0,0,0
            SetCameraTargetPosition(new Vector3(-4.71f, 1.75f, -4.57f));
        }

        private void ZoomCameraIn() {
            _targetZoomSize = defaultZoomInSize;
            SetCalculatedCameraTargetPosition(_lastCalculatedTargetPosition);
        }

        private void MovingCamera() {
            if (_movingEnabled) {
                transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, moveSmoothTime);
                //transform.position = Vector3.Lerp(transform.position, _targetPosition, moveSmoothTime * (moveSpeed/10) * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, _targetPosition) < 0.01f) {
                _movingEnabled = false;
            }
        }

        // Set position with already calculated offset
        // Use this only if in the target position is add a offset for camera
        public void SetCalculatedCameraTargetPosition(Vector3 targetPos) {
            _targetPosition = targetPos;
        }

        // Set position and add a offset to the camera
        // ROOT method
        public void SetCameraTargetPosition(Vector3 targetPos) {
            _targetPosition = targetPos + transform.TransformDirection(new Vector3(0, 0, zOffset));
        }

        public void SetCameraTargetPositionAndCameraZoom(Vector3 targetPos, float cameraZoom) {
            SetCameraTargetPosition(targetPos);
            _targetZoomSize = cameraZoom;
            _zoomingEnabled = true;
            _movingEnabled = true;
        }
        
        [InvokeButton]
        public void ResetCamera() {
            var temp = testingCamPos;
            testingCamPos = null;
            testingCamPos = temp;
            transform.position =  testingCamPos.position + transform.TransformDirection(new Vector3(0, 0, zOffset));
        }
    }
}