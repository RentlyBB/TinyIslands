using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using World.Enums;
using World.Interfaces;

namespace Systems {
    public class DiceEventsListener : MonoBehaviour {
        [SerializeField]
        public List<DiceEventHandlerSo> diceEvents = new();

        // [SerializeField]
        // [CanBeNull]
        // private UnityEvent<DiceFaces> methodToInvoke;

        public DiceFaces targetFace;

        private IInteractable _interactable;

        private void Awake() {
            TryGetComponent<IInteractable>(out _interactable);
        }

        private void OnEnable() {
            foreach (var diceEvent in diceEvents) diceEvent.OnEventRaised += PowerInteractable;
        }

        private void OnDisable() {
            foreach (var diceEvent in diceEvents) diceEvent.OnEventRaised -= PowerInteractable;
        }


        private void PowerInteractable(DiceFaces face) {
            // methodToInvoke?.Invoke(face);

            if (targetFace == face) {
                _interactable?.EnableInteraction();
            } else {
                _interactable?.DisableInteraction();
            }

            
        }
    }
}