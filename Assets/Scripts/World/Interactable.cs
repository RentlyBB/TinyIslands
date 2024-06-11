using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using EditorScripts;

namespace World {
    public enum InteractionType {
        PressButton,
        OnEnter,
        OnExit,
        OnEnterExit
    }

    public class Interactable : MonoBehaviour {
        
        public InteractionType interactionType;
        
        [Tooltip("Can be interact only once.")]
        public bool oneTimeActivation = false;

        [Header("Interact methods")]
        [Tooltip("Invoke interact method of interactable object.")]
        [Space]
        public UnityEvent onInteractAction;

        private bool _wasActivated = false;

        public void InteractableAction() {
            if (interactionType == InteractionType.PressButton) {
                InvokeInteractableAction();
            }
        }

        [InvokeButton]
        private void InvokeInteractableAction() {
            if (oneTimeActivation) {
                if (!_wasActivated) {
                    onInteractAction?.Invoke();
                    _wasActivated = true;
                }
            } else {
                onInteractAction?.Invoke();
            }
        }


        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            if (interactionType == InteractionType.OnEnter || interactionType == InteractionType.OnEnterExit) {
                InvokeInteractableAction();
            }
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;
            if (interactionType == InteractionType.OnExit || interactionType == InteractionType.OnEnterExit) {
                InvokeInteractableAction();
            }
        }
    }
}