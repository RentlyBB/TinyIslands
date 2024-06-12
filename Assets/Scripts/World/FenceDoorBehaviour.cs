using EditorScripts;
using UnityEngine;

namespace World {
    public class FenceDoorBehaviour : MonoBehaviour {
       
        [SerializeField] private Animator doorAnimator;

        private const string AnimOpenDoor = "Open";

        private bool _isClosed;
        private bool _isActivated;
       

        private void Start() {
            doorAnimator = GetComponent<Animator>();
        }

        [InvokeButton]
        public void ActivateDoor() {
            doorAnimator.SetTrigger(AnimOpenDoor);
        }
    }
}