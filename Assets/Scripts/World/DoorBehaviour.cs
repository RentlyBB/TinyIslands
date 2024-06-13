using System;
using System.Collections;
using System.Collections.Generic;
using EditorScripts;
using UnityEngine;

namespace World {
    public class DoorBehaviour : MonoBehaviour {

        [SerializeField] private Animator animator;

        private bool _isOpen = false;

        private void Start() {
            animator = GetComponent<Animator>();
        }

        [InvokeButton]
        public void Open() {
            _isOpen = !_isOpen;
            animator.SetBool("IsOpen", _isOpen);
        }
    }
}