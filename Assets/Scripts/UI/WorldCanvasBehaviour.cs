using System;
using System.Collections;
using System.Collections.Generic;
using InputCore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class WorldCanvasBehaviour : MonoBehaviour {
        private Camera _camera;

        public InputReaderSo inputReader;

        public Image img;

        private void OnEnable() {
            inputReader.ShowUI += SwitchUI;
            
            _camera = GetComponent<Canvas>().worldCamera;
            img = GetComponentInChildren<Image>();
        }

        private void OnDisable() {
            inputReader.ShowUI -= SwitchUI;
        }
        
        private void SwitchUI(bool enable) {
            img.enabled = enable;
        }

        private void Start() {
            SwitchUI(false);
            
            _camera = GetComponent<Canvas>().worldCamera;
            img = GetComponentInChildren<Image>();
        }

#if UNITY_EDITOR
        private void OnValidate() {
            _camera = GetComponent<Canvas>().worldCamera;

            if (_camera != null) {
                transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
            }
        }
        #endif

        // Update is called once per frame
        void Update() {
            transform.LookAt(transform.position + _camera.transform.rotation * Vector3.forward, _camera.transform.rotation * Vector3.up);
        }
    }
}