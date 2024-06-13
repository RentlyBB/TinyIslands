using System;
using System.Collections;
using System.Collections.Generic;
using EditorScripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace World {
    public class DoorBehaviour : MonoBehaviour {

        public bool toggleDoor = true;
        public bool startOpen = false;

        [SerializeField] private Animator doorAnimator;

        private const string AnimOpenDoor = "Open";
        private const string AnimCloseDoor = "Close";

        private bool _isClosed;
        private bool _isActivated;
       

        private void Start() {
            _isClosed = true;
            _isActivated = false;
            doorAnimator = GetComponent<Animator>();
            
            if(startOpen) doorAnimator.SetTrigger(AnimOpenDoor);
        }

        [InvokeButton]
        public void ActivateDoor() {

            if (!toggleDoor && _isActivated)
                return;

            doorAnimator.SetTrigger(_isClosed ? AnimOpenDoor : AnimCloseDoor);

            _isClosed = !_isClosed;
            _isActivated = true;
        }

        //WIP: TODO: This has to be complicated
        [InvokeButton]
        public void ChangeAnimSpeed() {
            doorAnimator.SetFloat("AnimSpeed",  doorAnimator.GetFloat("AnimSpeed") == 1 ? -1 : 1);
            
        }
    }
}