using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace World {
    public enum InteractionType {
        PressButton,
        OnEnter,
        OnExit,
        OnEnterExit
    }

    public class Interactable : MonoBehaviour {
        
        public InteractionType interactionType;

        [Space]
        public UnityEvent onInteractAction;

        public void InteractableAction() {
            if (interactionType == InteractionType.PressButton) {
                onInteractAction?.Invoke();
            }
        }

        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            if (interactionType == InteractionType.OnEnter || interactionType == InteractionType.OnEnterExit) {
                onInteractAction?.Invoke();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;
            if (interactionType == InteractionType.OnExit || interactionType == InteractionType.OnEnterExit) {
                onInteractAction?.Invoke();
            }
        }
    }
}