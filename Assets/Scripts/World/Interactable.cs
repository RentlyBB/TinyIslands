using System.Collections;
using System.Collections.Generic;
using InputCore;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

    public class Interactable : MonoBehaviour {
        
        [SerializeField] private UnityEvent onInteractAction;

        public void InteractableAction() {
            onInteractAction?.Invoke();
        }
        
    }
