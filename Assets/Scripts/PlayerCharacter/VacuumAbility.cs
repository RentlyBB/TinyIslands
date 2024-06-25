using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using World;

namespace PlayerCharacter {
    public class VacuumAbility : MonoBehaviour {
        //PushBalls ability variables
        public float radius;

        [Range(0, 360)]
        public float angle;

        public LayerMask targetMask;

        public bool CanSeeVacuumableObject { get; private set; } = false;
        
        public Collider[] RayData { get; private set; }
        
        private void Start() {
            
            //Debug in editor
            StartCoroutine(FOVRoutine());
        }

        private void Update() {
            VacuumObjects();
        }

        private IEnumerator FOVRoutine() {
            WaitForSeconds wait = new WaitForSeconds(0.1f);
            while (true) {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        public void VacuumObjects() {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
            
            if (rangeChecks.Length != 0) {
                foreach (Collider coll in rangeChecks) {
                    Transform target = coll.transform;
            
                    Vector3 directionToTarget = (target.position - transform.position).normalized;
            
                    // If Collider (target) is in the angle – Do something
                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                        // TODO: Vacuuming the objects
                        target.GetComponent<TrashBehaviour>().TrashActivate();
                    }
                }
            }
        } 

        public void FieldOfViewCheck() {
            RayData = Physics.OverlapSphere(transform.position, radius, targetMask);
            
            if (RayData.Length != 0) {
                
                foreach (Collider coll in RayData) {
                    Transform target = coll.transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;
            
                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                        CanSeeVacuumableObject = true;
                    } else {
                        CanSeeVacuumableObject = false;
                    }
                }
            } else if (CanSeeVacuumableObject) {
                CanSeeVacuumableObject = false;
            }
        }
    }
}