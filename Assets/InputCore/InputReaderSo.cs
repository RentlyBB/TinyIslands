using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InputCore {
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReaderSo : ScriptableObject, GameInput.IGameplayActions {

        public GameInput GameInput;

        private void OnEnable() {
            if (GameInput == null) {
                GameInput = new GameInput();
                GameInput.Gameplay.SetCallbacks(this);
            }

            GameInput.Gameplay.Enable();
        }

        private void OnDisable() {
            GameInput.Gameplay.Disable();
        }

        /*GAMEPLAY*/
        public void OnMovement(InputAction.CallbackContext context) {
            Movement?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) {
                Interact?.Invoke();
            }
        }

        public void OnZoomOut(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) {
                ZoomOut?.Invoke();
            } else if (context.phase == InputActionPhase.Canceled) {
                ZoomIn?.Invoke();
            }
        }
        // Events for each player input
        public event UnityAction<Vector2> Movement = delegate { };
        public event UnityAction Interact = delegate { };

        public event UnityAction ZoomOut = delegate { };
        public event UnityAction ZoomIn = delegate { };

        public void EnableGameplayInput() {
            GameInput.Gameplay.Enable();
        }
    }
}