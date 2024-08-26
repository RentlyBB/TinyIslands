using EditorScripts.InvokeButton;
using UnityEngine;
using World.AbstractClasses;
using World.Enums;

namespace World {
    [RequireComponent(typeof(BoxCollider))]
    public class DoorBehaviour : Interactable {

        private Animator _animator;

        private BoxCollider _boxCollider;
        
        private bool _isOpen;

        
        private void Start() {
            TryGetComponent<Animator>(out _animator);
            TryGetComponent<BoxCollider>(out _boxCollider);
        }

        //Root method
        [InvokeButton]
        public void Toggle() {
            if(currentState == InteractableStates.Disabled) return;
            
            _isOpen = !_isOpen;
            _animator.SetBool("IsOpen", _isOpen);
            _boxCollider.enabled = !_isOpen;
        }

        public void Open() {
            _isOpen = true;
            _animator.SetBool("IsOpen", _isOpen);
            _boxCollider.enabled = !_isOpen;
        }

        public void Close() {
            _isOpen = false;
            _animator.SetBool("IsOpen", _isOpen);
            
            // Door are closed, thats why box collider has to be ON
            // _isOpen is now false and we need to negate it because we want true for box collider
            _boxCollider.enabled = !_isOpen;
        }
        
        public override void Interact() {
            Toggle();
        }
    }
}