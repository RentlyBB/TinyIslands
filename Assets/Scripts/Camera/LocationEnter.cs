using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.Serialization;

public class LocationEnter : MonoBehaviour {

    [Header("Broadcasting Events")]
    [SerializeField] private Vector3GameEvent OnAreaEntered = default(Vector3GameEvent);

    public Transform cameraTargetPoint;
    
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Entering new area: " + transform.name);
            OnAreaEntered.Raise(cameraTargetPoint.position);
        }
    }

}