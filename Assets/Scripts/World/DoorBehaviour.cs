using EditorScripts.InvokeButton;
using UnityEngine;
using UnityEngine.Serialization;
using World.AbstractClasses;
using World.Enums;
using DoorState = World.Enums.DoorUtils.DoorState;

namespace World {
    [RequireComponent(typeof(BoxCollider))]
    public class DoorBehaviour : Interactable {
        private Animator _animator;

        private BoxCollider _boxCollider;

        public DoorState currentDoorState;

        private void Start() {
            TryGetComponent<Animator>(out _animator);
            TryGetComponent<BoxCollider>(out _boxCollider);

            if (currentDoorState.Equals(null)) {
                currentDoorState = DoorState.Closed;
            }

        }

        //Root method
        [InvokeButton]
        public void Toggle() {
            SwitchDoorState();
            _animator.SetBool("IsOpen", DoorUtils.EvaluateDoorState(currentDoorState));
            _boxCollider.enabled = !DoorUtils.EvaluateDoorState(currentDoorState);
        }

        public void Open() {
            currentDoorState = DoorState.Opened;
            _animator.SetBool("IsOpen", DoorUtils.EvaluateDoorState(currentDoorState));
            _boxCollider.enabled = !DoorUtils.EvaluateDoorState(currentDoorState);
        }

        public void Close() {
            currentDoorState = DoorState.Closed;
            _animator?.SetBool("IsOpen", DoorUtils.EvaluateDoorState(currentDoorState));

            // Door are closed, thats why box collider has to be ON
            // _isOpen is now false and we need to negate it because we want true for box collider
            _boxCollider.enabled = !DoorUtils.EvaluateDoorState(currentDoorState);
        }

        public override void Interact() {
            switch (interactableMode) {
                case InteractableModes.OneTimeActivation:
                    OneTimeActivation();
                    break;
                case InteractableModes.ToggleOnActivation:
                    ToggeOnActivation();
                    break;
                case InteractableModes.ToggleOnDisable:
                    ToggleOnDisable();
                    break;
                case InteractableModes.ToogleOnActivationAndDisable:
                    ToggleOnActivationAndDiable();
                    break;
            }
        }

        private void OneTimeActivation() {
            Debug.LogWarning("Interactable mode: OneTimeActivation is not implemented;");
        }

        private void ToggleOnActivationAndDiable() {
            if (interactableState == InteractableStates.Disabled) {
                Close();
            } else if (interactableState == InteractableStates.Enabled) {
                Open();
            }
        }

        private void ToggeOnActivation() {
            if (interactableState == InteractableStates.Enabled) {
                Toggle();
            }
        }

        private void ToggleOnDisable() {
            if (interactableState == InteractableStates.Disabled) {
                Toggle();
            }
        }

        private void SwitchDoorState() {
            switch (currentDoorState) {
                case DoorState.Closed:
                    currentDoorState = DoorState.Opened;
                    break;
                case DoorState.Opened:
                    currentDoorState = DoorState.Closed;
                    break;
            }
        }
    }
}