using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public Transform target; // The target object around which the camera will rotate
    
    [Space]    
    [Header("Zoom Settings")]
    public float zoomSpeed = 2.0f; // Speed of zooming
    public float minZoomSize = 1.0f; // Minimum orthographic size (zoom out limit)
    public float maxZoomSize = 10.0f; // Maximum orthographic size (zoom in limit)
    public float zoomSmoothing = 5.0f; // Smoothing factor for zooming
    
    public float zOffset = -10;

    private float targetZoomSize = 50;

    private void LateUpdate() {
        
        ZoomCamera();

    }


    private void ZoomCamera() {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0) {
            // Calculate the target orthographic size based on the zoom input
            targetZoomSize = Mathf.Clamp(targetZoomSize - zoomInput * zoomSpeed, minZoomSize, maxZoomSize);
        }

        // Smoothly interpolate towards the target orthographic size
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoomSize, zoomSmoothing * Time.deltaTime);
    }

    //Localy can move on X and Y axes
    public void MoveCameraTo(Vector3 targetPos) {
        transform.position = targetPos;
        transform.localPosition +=  transform.TransformDirection (new Vector3(0, 0, zOffset));

        Debug.Log("Receive position: " + targetPos);
        
    }

}