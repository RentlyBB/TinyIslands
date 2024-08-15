using System;
using System.Collections;
using System.Collections.Generic;
using EditorScripts;
using EditorScripts.InvokeButton;
using ScriptableObjects;
using UnityEngine;
using World.AbstractClasses;
using World.Enums;
using World.Structs;

namespace World {
    public class PowerCoreBehaviour : Interactable {

        [Header("––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––––")]
        [Header("PowerCoreBehaviour Settings")]
        
        [Space]
        [Header("Color at which Power Core will have at the start of the game.")]
        public PowerCoreColors startingColor;
        
        public MeshRenderer meshRenderer;
        
        [Space]
        [Header("PowerCoreEventsList")]
        [SerializeField]
        public List<PowerCoreEventSo> powerCoreEvents;
        
        private PowerCoreColors _currentColor;

#if UNITY_EDITOR

        public void OnValidate() {
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.sharedMaterial.color = PowerCoreUtils.GetColor(startingColor);
        }
        
#endif

        public void Start() {
            _currentColor = startingColor - 1;
            Interact();
        }

        [InvokeButton]
        public void ChangeColor() {

            var list = PlayerManager.Instance.unlockColors; 
            
            var i = list.IndexOf(_currentColor);

            if (i >= list.Count - 1) {
                i = 0;
            } else {
                i += 1;
            }

            _currentColor = list[i];
            meshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            meshRenderer.sharedMaterial.color = PowerCoreUtils.GetColor(_currentColor);
            
            foreach (var powerCoreEventSo in powerCoreEvents) {
                powerCoreEventSo?.RaiseEvent(_currentColor);
            }
        }

        public override void Interact() {
            ChangeColor();
            
            foreach (var powerCoreEventSo in powerCoreEvents) {
                powerCoreEventSo?.RaiseEvent(_currentColor);
            }
        }
    }
}