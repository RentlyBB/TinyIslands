using ScriptableObjects.EnterLocationEvents;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.Listeners {
    public class EnterLocationEventListener : MonoBehaviour {

        [SerializeField] private EnterLocationEventSo enterLocationEvent;

        [SerializeField] private UnityEvent<Vector3, float> onLocationEntered;

        private void OnEnable() {
            enterLocationEvent.EnterLocation += InvokeMethod;
        }

        private void OnDisable() {
            enterLocationEvent.EnterLocation -= InvokeMethod;
        }

        private void InvokeMethod(Vector3 position, float cameraZoom) {
            onLocationEntered?.Invoke(position, cameraZoom);
        }

    }
}