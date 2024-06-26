using System;
using System.Collections;
using System.Collections.Generic;
using InputCore;
using PlayerCharacter;
using UnityEngine;

namespace CameraScripts {
    public class CameraController : MonoBehaviour {

        private Camera _camera;

        public InputReaderSo inputReader;
    
        [Space]    
        [Header("Zoom Settings")]
        public float zoomSpeed = 25.0f; // Speed of zooming
        public float minZoomSize = 1.0f; // Minimum orthographic size (zoom out limit)
        public float maxZoomSize = 50.0f; // Maximum orthographic size (zoom in limit)
        public float zoomSmoothing = 5.0f; // Smoothing factor for zooming
        public float startZoom = 10f;
        public float zoomOutSize = 50.0f;
        
        public bool canScrollZooming = false;

        [Space]
        [Header("Camera Movement Settings")]
        public float smoothTime = 0.3f; // Time for the smooth 
        public float zOffset = -100;

        private Vector3 _velocity = Vector3.zero;
        private float _targetZoomSize = 50;
        private Vector3 _targetPosition;
        private Vector3 _lastTargetPosition;

        private void Start() {
            _camera = GetComponent<CharacterCamera>().Camera;
            _targetZoomSize = startZoom;
            _camera.orthographicSize = _targetZoomSize;

        }
        
        private void OnEnable() {
            inputReader.ZoomOut += ZoomCameraOut;
            inputReader.ZoomIn += ZoomCameraIn;
        }

        private void OnDisable() {
            inputReader.ZoomOut -= ZoomCameraOut;
            inputReader.ZoomIn -= ZoomCameraIn;
        }

        private void LateUpdate() {
            ZoomingCamera();
            transform.position = Vector3.SmoothDamp(transform.position, _targetPosition, ref _velocity, smoothTime);
        }
        
        
        private void ZoomCameraOut() {
            _targetZoomSize = zoomOutSize;
            SetCameraTargetPosition(Vector3.zero);
        }
        
        private void ZoomCameraIn() {
            _targetZoomSize = startZoom;
            SetCameraTargetPosition(_lastTargetPosition);
        }


        private void ZoomingCamera() {
           
            if (canScrollZooming) {
                float zoomInput = Input.GetAxis("Mouse ScrollWheel");
                if (zoomInput != 0) {
                    // Calculate the target orthographic size based on the zoom input
                    _targetZoomSize = Mathf.Clamp(_targetZoomSize - zoomInput * zoomSpeed, minZoomSize, maxZoomSize);
                }
            }

            // Smoothly interpolate towards the target orthographic size
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoomSize, zoomSmoothing * Time.deltaTime);
        }

        public void SetCameraTargetPosition(Vector3 targetPos) {
            _lastTargetPosition = _targetPosition;
            _targetPosition = targetPos + transform.TransformDirection(new Vector3(0, 0, zOffset));
        }
    }
}