using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World {
    public class TrashBehaviour : MonoBehaviour {
        private Vector3 _targetPosition;

        private Rigidbody _rb;

        public float speed = 10;

        public float distance;


        private void Awake() {
            _rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            // if (_targetPosition != Vector3.zero) {
            //     var dir = _targetPosition - transform.position;
            //
            //     _rb.AddForce(Time.fixedDeltaTime * speed * dir, ForceMode.Force);
            // }
        }

        public void TrashActivate(Vector3 targetPosition) {
            _targetPosition = targetPosition;
            distance = Vector3.Distance(transform.position, targetPosition);
            if (distance < 1f) {
                Destroy(this.transform.gameObject);
            }
        }
    }
}