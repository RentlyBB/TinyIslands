using EditorScripts;
using EditorScripts.InvokeButton;
using UnityEngine;
using UnityEngine.Events;

namespace Systems {
    public enum InteractionType {
        PressButton,
        OnEnter,
        OnExit,
        OnEnterExit,
        None
    }

    public class Activator : MonoBehaviour {
        public InteractionType interactionType;

        [Tooltip("Can be interact only once.")]
        public bool oneTimeActivation;

        [Header("Interact methods")]
        [Tooltip("Invoke interact method of interactable object.")]
        [Space]
        public UnityEvent onInteractAction;

        private bool _wasActivated;


        private void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;

            if (interactionType == InteractionType.OnEnter || interactionType == InteractionType.OnEnterExit) InvokeInteractableAction();
        }

        private void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;

            if (interactionType == InteractionType.OnExit || interactionType == InteractionType.OnEnterExit) InvokeInteractableAction();
        }

        public void InteractableAction() {
            if (interactionType == InteractionType.PressButton) InvokeInteractableAction();
        }

        
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
    }
}