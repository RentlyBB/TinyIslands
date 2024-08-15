using System;
using System.Collections;
using System.Collections.Generic;
using EditorScripts;
using ScriptableObjects;
using UnityEngine;
using World.Enums;
using World.Interfaces;
using World.Structs;

namespace World {
    public class PowerCoreBehaviour : MonoBehaviour, IInteractable {

        [Header("Just for testing")]
        public PowerCoreColors colorToChange;
        
        private PowerCoreColors _currentColor;

        public MeshRenderer meshRenderer;
        
        [Space]
        [Header("PowerCoreEventsList")]
        [SerializeField]
        public List<PowerCoreEventSo> powerCoreEvents;

#if UNITY_EDITOR
        
        private void OnValidate() {
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.sharedMaterial.color = PowerCoreUtils.GetColor(_currentColor);
        }
        
#endif


        [InvokeButton]
        public void ChangeColor() {
            _currentColor = colorToChange;
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.sharedMaterial.color = PowerCoreUtils.GetColor(_currentColor);
        }

        public void EnableInteraction() {
            //
        }

        public void DisableInteraction() {
            //
        }

        public void Interact() {
            foreach (var powerCoreEvent in powerCoreEvents) {
                powerCoreEvent?.RaiseEvent(PowerCoreColors.Blue);
            }
        }
    }
}