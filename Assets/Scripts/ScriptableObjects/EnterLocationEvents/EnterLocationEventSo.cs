using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects.EnterLocationEvents {
    [CreateAssetMenu(fileName = "EnterLocationEvent", menuName = "Game/EnterLocationEvent", order = 0)]
    public class EnterLocationEventSo : ScriptableObject {
        
        public event UnityAction<Vector3, float> EnterLocation = delegate { };
        
        public void RaiseEvent(Vector3 location, float cameraZoom) {
            EnterLocation?.Invoke(location, cameraZoom);
        }
        
    }
}