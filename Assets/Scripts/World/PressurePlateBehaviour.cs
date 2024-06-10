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
        
        public void ActivatePressurePlate() {
                Debug.Log("Jsem na pressure platu!!");
                onActivated?.Invoke();
        }
    }
}