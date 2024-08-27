using EditorScripts.InvokeButton;
using UnityEngine;
using ScriptableObjects.EnterLocationEvents;

namespace CameraScripts {
    public class LocationEnter : MonoBehaviour {

        [SerializeField] private EnterLocationEventSo onAreaEntered;
        public Transform cameraTargetPoint;
        [Range(10f,25f)]
        public float cameraZoom = 15f;


        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                onAreaEntered?.RaiseEvent(cameraTargetPoint.position, cameraZoom);
            }
        }


        [InvokeButton]
        private void CenterCamera() {
            onAreaEntered?.RaiseEvent(cameraTargetPoint.position, cameraZoom);

        }
    }
}