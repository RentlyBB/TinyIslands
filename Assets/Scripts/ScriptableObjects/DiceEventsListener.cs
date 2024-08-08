using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjects {
    public class DiceEventsListener : MonoBehaviour {
        
        [SerializeField]
        public List<DiceEventHandlerSo> diceEvents = new List<DiceEventHandlerSo>();

        [SerializeField]
        [CanBeNull]
        private UnityEvent<String> methodToInvoke;

        private void OnEnable() {
            foreach (var diceEvent in diceEvents) {
                diceEvent.OnEventRaised += PowerInteractable;
            }
        }

        private void OnDisable() {
            foreach (var diceEvent in diceEvents) {
                diceEvent.OnEventRaised -= PowerInteractable;
            }
        }

        private void PowerInteractable(String text) {
            methodToInvoke?.Invoke(text);
        }
    }
}