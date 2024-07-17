using UnityEngine;
using UnityEngine.Events;

namespace World {
    public class Activator : MonoBehaviour {

        [Header("On Pressure Plate Activated")]
        [Space]
        [Tooltip("Invoke methods which should be triggered if pressure plate is activated.")]
        public UnityEvent onActivated;

        //TODO: cooldown for activation
        private readonly bool _canBeActivated = true;

        public void Activate() {
            if (_canBeActivated) {
                onActivated?.Invoke();
            }
        }
    }
}