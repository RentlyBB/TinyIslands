using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

    public class Interactable : MonoBehaviour {
        
        [SerializeField] private UnityEvent onInteractAction;

        private bool _readyToInteract = true;

        public void InteractableAction() {
            if(!_readyToInteract) return;
            
            _readyToInteract = false;
            onInteractAction?.Invoke();
        }

        public void ReadyToInteract() {
            _readyToInteract = true;
        }

    }
