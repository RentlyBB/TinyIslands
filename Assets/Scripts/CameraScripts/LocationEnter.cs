using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using UnityEngine.Serialization;

namespace CameraScripts {
    public class LocationEnter : MonoBehaviour {

        [Header("Broadcasting Events")]
        [SerializeField] private Vector3GameEvent onAreaEntered = default(Vector3GameEvent);

        public Transform cameraTargetPoint;
    
    
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                Debug.Log("Entering new area: " + transform.name);
                onAreaEntered.Raise(cameraTargetPoint.position);
            }
        }

    }
}