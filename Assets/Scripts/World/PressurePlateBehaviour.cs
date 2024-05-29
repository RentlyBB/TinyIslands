using System;
using System.Collections;
using System.Collections.Generic;
using InputCore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace World {
    public class PressurePlateBehaviour : MonoBehaviour {

        public UnityEvent onActivated;
        
        private bool _activated = false;
        
        public void ActivatePressurePlate() {
            if (!_activated) {
                Debug.Log("Jsem na pressure platu!!");
                _activated = true;
                onActivated?.Invoke();
            }
        }

        

    }
}