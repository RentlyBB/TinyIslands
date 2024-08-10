using EditorScripts;
using UnityEngine;
using UnityEngine.Serialization;
using World.Interfaces;

namespace World {
    [RequireComponent(typeof(BoxCollider))]
    public class DoorBehaviour : MonoBehaviour, IInteractable {
        
        private Animator _animator;

        private BoxCollider _boxCollider;
        

        private bool _isOpen;

        public bool isEnabled = true;
        
        private void Start() {
            TryGetComponent<Animator>(out _animator);
            TryGetComponent<BoxCollider>(out _boxCollider);
        }


        //Root method
        [InvokeButton]
        public void Toggle() {
            if(!isEnabled) return;
            
            _isOpen = !_isOpen;
            _animator.SetBool("IsOpen", _isOpen);
            _boxCollider.enabled = !_isOpen;
        }

        [InvokeButton]
        public void Open() {
            _isOpen = true;
            _animator.SetBool("IsOpen", _isOpen);
            _boxCollider.enabled = !_isOpen;
        }

        [InvokeButton]
        public void Close() {
            _isOpen = false;
            _animator.SetBool("IsOpen", _isOpen);
            
            // Door are closed, thats why box collider has to be ON
            // _isOpen is now false and we need to negate it because we want true for box collider
            _boxCollider.enabled = !_isOpen;
        }

        public void EnableInteraction() {
            isEnabled = true;
        }

        public void DisableInteraction() {
            isEnabled = false;
        }

        public void Interact() {
            Toggle();
        }
    }
}