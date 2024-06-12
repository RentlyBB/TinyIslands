using System;
using System.Collections;
using System.Collections.Generic;
using InputCore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace World {
    public class PressurePlateBehaviour : MonoBehaviour {

        [Header("On Pressure Plate Activated")]
        [Space]
        [Tooltip("Invoke methods which should be triggered if pressure plate is activated.")]
        public UnityEvent onActivated;

        private bool _canBeActivated = true;

        public void ActivatePressurePlate() {
            if (_canBeActivated) {
                onActivated?.Invoke();
            }
        }
    }
}