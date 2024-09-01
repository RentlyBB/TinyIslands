using System;
using System.Collections.Generic;
using UnityEngine;
using World.Enums;

namespace World.AbstractClasses {
    public abstract class Interactable : MonoBehaviour {
        [Header("Interactable Settings")]
        [Header("Current state of Interactable")]
        public InteractableStates interactableState = InteractableStates.Enabled;

        public InteractableModes interactableMode = InteractableModes.ToggleOnActivation;

        [Space]
        public bool interactOnPowered = false;

        public void SwitchState(InteractableStates state) {
            interactableState = state;
        }

        public virtual void Interact() {
            Debug.Log("Interact is not implemented!");
        }
    }
}