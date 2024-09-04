using System;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using World.AbstractClasses;
using World.Enums;

namespace World {
    [RequireComponent(typeof(Interactable))]
    public class PowerCoreListener : MonoBehaviour {
        [Space]
        [Header("Color which enable the Interactable object")]
        public PowerCoreColors powerSupplyColor;

        [Space]
        [Header("Listening events from Power Cores")]
        [SerializeField]
        public List<PowerCoreEventSo> powerCoreEvent = new List<PowerCoreEventSo>();

        private Interactable _interactable;

        private void Awake() {
            TryGetComponent<Interactable>(out _interactable);
        }

        private void OnEnable() {
            foreach (var powerCoreEventSo in powerCoreEvent) powerCoreEventSo.OnEventRaised += EvaluatePowerCore;
        }

        private void OnDisable() {
            foreach (var powerCoreEventSo in powerCoreEvent) powerCoreEventSo.OnEventRaised -= EvaluatePowerCore;
        }

        private void EvaluatePowerCore(PowerCoreColors color) {
            if (powerSupplyColor == PowerCoreColors.None) return;

            if (powerSupplyColor == color || powerSupplyColor == PowerCoreColors.Any) {
                _interactable.SwitchState(InteractableStates.Enabled);
            } else {
                _interactable.SwitchState(InteractableStates.Disabled);
            }

            if (_interactable.interactableMode == InteractableModes.InteractOnDisabled ||
                _interactable.interactableMode == InteractableModes.InteractOnEnabled ||
                _interactable.interactableMode == InteractableModes.InteractOnEnabledAndDisabled) {
                _interactable?.ValidateInteraction();
            }
        }
    }
}