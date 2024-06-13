using System;
using InputCore;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerCharacter {
    public class PlayerInputManager : MonoBehaviour {
        public InputReaderSo inputReader;
        public CharacterMovementController character;
        public CharacterCamera characterCamera;

        [SerializeField] private bool hideCursor = false;

        private PlayerCharacterInputs _characterInputs = new PlayerCharacterInputs();

        private void OnEnable() {
            inputReader.Movement += Move;
        }
        

        private void OnDisable() {
            inputReader.Movement -= Move;
        }


        void Start() {

            
            
            if (hideCursor) {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Ignore the character's collider(s) for camera obstruction checks
            characterCamera.IgnoredColliders.Clear();
            characterCamera.IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());
        }

        void Update() {
            if (hideCursor && Input.GetMouseButtonDown(0)) {
                Cursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
            
        }

        private void LateUpdate() {

            //TODO: Camera Handling Should Be Done Here 
            HandleCameraInput();
        }

        private void HandleCameraInput() {
            //TODO: Zoom out on button press to get view on whole planet/city
        }

        private void HandleCharacterInput() {

            // Build the CharacterInputs struct
            _characterInputs.CameraRotation = characterCamera.Transform.rotation;

            // Apply inputs to character
            character.SetInputs(ref _characterInputs);
        }

        /*INPUT LISTENERS*/
        void Move(Vector2 movementDirection) {
            _characterInputs.MoveAxisRight = movementDirection.x;
            _characterInputs.MoveAxisForward = movementDirection.y;
        }
        
        
    }
}