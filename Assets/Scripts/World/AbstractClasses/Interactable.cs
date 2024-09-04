using System;
using System.Collections.Generic;
using UnityEngine;
using World.Enums;

namespace World.AbstractClasses {
    public abstract class Interactable : MonoBehaviour {
        [Header("Interactable Settings")]
        [Header("Current state of Interactable")]
        [Tooltip("Enabled – Is powering by power core | Disabled – Is not powering by power core")]
        public InteractableStates interactableState = InteractableStates.Enabled;

        public InteractableModes interactableMode = InteractableModes.ToggleOnEnabled;

        protected bool InteractableStateChanged = false;

        public void SwitchState(InteractableStates state) {
            interactableState = state;
            InteractableStateChanged = true;
        }

        public virtual void Interact() {
            Debug.Log("Interact method is not implemented yet!");
        }
        
        public void Interact(InteractionInputType interactionInputType ) {
            switch (interactionInputType) {
                case InteractionInputType.PlayerInput:
                    PlayerInteract();
                    break;
                case InteractionInputType.PowerCoreInput:
                    PowerCoreInteract();
                    break;
                case InteractionInputType.InteractableInput:
                    InteactableInteract();
                    break;
            }
        }

        public virtual void PlayerInteract() {
            Debug.Log("Player Interact method is not implemented yet!");
        }
        
        public virtual void PowerCoreInteract() {
            Debug.Log("PowerCore Interact method is not implemented yet!");
        }
        
        public virtual void InteactableInteract() {
            Debug.Log("Interactable Interact method is not implemented yet!");
        }

    }
}