using UnityEngine;

namespace KinematicCharacterController.PlayerCharacter {
    public class PlayerInputManager : MonoBehaviour {
        public CharacterMovementController Character;
        public CharacterCamera CharacterCamera;

        [SerializeField] private bool hideCursor = false;

        //Movement Inputs
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";

        void Start() {
            if (hideCursor) {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        void Update() {
            if (hideCursor && Input.GetMouseButtonDown(0)) {
                Cursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
        }

        private void LateUpdate() {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null) {

                Character.Motor.AttachedRigidbody.TryGetComponent<PhysicsMover>(out PhysicsMover physicsMover);
                
                CharacterCamera.PlanarDirection =
                    physicsMover.RotationDeltaFromInterpolation *
                    CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3
                    .ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

            //TODO: Camera Handling Should Be Done Here 
            // HandleCameraInput();
        }

        private void HandleCharacterInput() {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            // characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            // characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            // characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }
    }
}