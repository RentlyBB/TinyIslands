using System.Collections.Generic;
using ScriptableObjects;
using ScriptableObjects.DiceEvents;
using UnityEngine;
using World.AbstractClasses;
using World.Enums;
using World.Interfaces;

namespace Systems {
    public class DiceEventsListener : MonoBehaviour {
        [SerializeField]
        public List<DiceEventHandlerSo> diceEvents = new();

        public DiceFaces targetFace;

        private Interactable _interactable;

        private void Awake() {
            TryGetComponent<Interactable>(out _interactable);
        }

        private void OnEnable() {
            foreach (var diceEvent in diceEvents) diceEvent.OnEventRaised += PowerInteractable;
        }

        private void OnDisable() {
            foreach (var diceEvent in diceEvents) diceEvent.OnEventRaised -= PowerInteractable;
        }


        private void PowerInteractable(DiceFaces face) {
            // methodToInvoke?.Invoke(face);
            
            Debug.LogWarning("Deprecated Method – Do nothing");

            if (targetFace == face) {
                //_interactable?.EnableInteraction();
            } else {
               // _interactable?.DisableInteraction();
            }

            
        }
    }
}