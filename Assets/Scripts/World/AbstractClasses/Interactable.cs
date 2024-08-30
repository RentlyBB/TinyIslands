using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
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

        [Space]
        public bool interactOnPowered = false;

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
                if (currentState == InteractableStates.Disabled) {
                    SwitchState(InteractableStates.Enabled);
                }
            } else {
                if (currentState == InteractableStates.Enabled) {
                    SwitchState(InteractableStates.Disabled);
                }
            }
            
            Interact();
        }

        public virtual void Interact() {
            Debug.Log("Interact is not implemented!");
        }
    }
}