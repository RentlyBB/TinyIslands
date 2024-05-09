using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target; // The target object around which the camera will rotate
    public float rotationSpeed = 1.0f; // Speed of rotation
    public float zoomSpeed = 2.0f; // Speed of zooming
    public float minZoomSize = 1.0f; // Minimum orthographic size (zoom out limit)
    public float maxZoomSize = 10.0f; // Maximum orthographic size (zoom in limit)
    public float zoomSmoothing = 5.0f; // Smoothing factor for zooming

    private float currentRotation = 0f;
    private float targetZoomSize = 50;

    private void Start() {
        transform.LookAt(target);
    }

    private void LateUpdate() {

        RotateCamera();
        ZoomCamera();
    }

    private void RotateCamera() {
        if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(target.position, new Vector3(0.0f, 1.0f, 0.0f), 20 * Time.deltaTime * rotationSpeed);
        } else if (Input.GetKey(KeyCode.E)) {
            transform.RotateAround(target.position, new Vector3(0.0f, -1.0f, 0.0f),
                20 * Time.deltaTime * rotationSpeed);
        }
    }

    private void ZoomCamera() {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0) {
            // Calculate the target orthographic size based on the zoom input
            targetZoomSize = Mathf.Clamp(targetZoomSize - zoomInput * zoomSpeed, minZoomSize, maxZoomSize);
        }

        // Smoothly interpolate towards the target orthographic size
        Camera.main.orthographicSize =
            Mathf.Lerp(Camera.main.orthographicSize, targetZoomSize, zoomSmoothing * Time.deltaTime);
    }
}