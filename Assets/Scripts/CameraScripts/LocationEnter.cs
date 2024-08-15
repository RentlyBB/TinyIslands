using EditorScripts.InvokeButton;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace CameraScripts {
    public class LocationEnter : MonoBehaviour {
        [Header("Broadcasting Events")]
        [SerializeField]
        private Vector3GameEvent onAreaEntered = default;

        public Transform cameraTargetPoint;


        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player"))
                //Debug.Log("Entering new area: " + transform.name);
                onAreaEntered.Raise(cameraTargetPoint.position);
        }


        [InvokeButton]
        private void CenterCamera() {
            onAreaEntered.Raise(cameraTargetPoint.position);
        }
    }
}