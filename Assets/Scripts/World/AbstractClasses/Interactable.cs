using System;
using System.Collections.Generic;
using UnityEngine;
using World.Enums;

namespace World.AbstractClasses {
    public abstract class Interactable : MonoBehaviour {
        [Header("Interactable Settings")]
        [Header("Current state of Interactable")]
        [Tooltip("Enabled – Is powering by power core | Disabled – Is not powering by power core")]
        public InteractableStates currentInteractableState = InteractableStates.Enabled;

        public InteractableModes interactableMode = InteractableModes.InteractOnEnabled;

        protected bool InteractableStateChanged = false;

        public void SwitchState(InteractableStates state) {
            if (currentInteractableState != state) {
                currentInteractableState = state;
                InteractableStateChanged = true;
            }
        }

        public virtual void Interact() {
            Debug.Log("Interact method is not implemented yet!");
        }

        // This method should be called if any object wants to interact with interactable
        public virtual void ValidateInteraction() {
            switch (interactableMode) {
                case InteractableModes.DirectInteraction:
                    //Do not care if enabled or disabled – interact if enabled or disabled
                    Interact();
                    break;
                case InteractableModes.DirectStateInteraction:
                    // Care if Enabled or diabled – Interact only if enabled
                    if (currentInteractableState == InteractableStates.Enabled) {
                        Interact();
                    }

                    break;
                case InteractableModes.InteractOnDisabled:
                    // Only interact if state is changed from enabled to disabled
                    if (InteractableStateChanged && currentInteractableState == InteractableStates.Disabled) {
                        Interact();
                        InteractableStateChanged = false;
                    }

                    break;
                case InteractableModes.InteractOnEnabled:
                    // Only interact if state is chagned from disabled to enabled
                    if (InteractableStateChanged && currentInteractableState == InteractableStates.Enabled) {
                        Interact();
                        InteractableStateChanged = false;
                    }

                    break;
                case InteractableModes.InteractOnEnabledAndDisabled:
                    // Interact if state is changed from disabled to enabled or vise versa
                    if (InteractableStateChanged) {
                        Interact();
                        Debug.Log("InteractOnEnabledAndDisabled done");
                        InteractableStateChanged = false;
                    }

                    break;
            }
        }
    }
}