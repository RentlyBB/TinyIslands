using System;
using System.Collections.Generic;
using EditorScripts.HideIf;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using World.Enums;

namespace World.AbstractClasses {
    
    public abstract class Interactable : MonoBehaviour {
        
        [Header("––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––")]
        [Header("Interactable Settings")]
        
        [Space]
        [Header("Current state of Interactable")]
        public InteractableStates currentState = InteractableStates.Enabled;

        [Space]
        [Header("Color which enable the Interactable object")]
        public PowerCoreColors powerSupplyColor;
        
        [Space]
        [Header("Listening events from Power Cores")]
        [SerializeField]
        public List<PowerCoreEventSo> powerCoreEvent = new List<PowerCoreEventSo>();

        private void OnEnable() {
            foreach (var powerCoreEventSo in powerCoreEvent) powerCoreEventSo.OnEventRaised += EvaluatePowerCore;
        }

        private void OnDisable() {
            foreach (var powerCoreEventSo in powerCoreEvent) powerCoreEventSo.OnEventRaised -= EvaluatePowerCore;
        }

        public void SwitchState(InteractableStates state) {
            currentState = state;
        }

        public void EvaluatePowerCore(PowerCoreColors color) {
           if (powerSupplyColor == color || powerSupplyColor == PowerCoreColors.Any) {
               SwitchState(InteractableStates.Enabled);
           } else {
               SwitchState(InteractableStates.Disabled);
           }
        }

        public virtual void Interact() {
            Debug.Log("Interact is not implemented!");
        }
        
        
    }
}