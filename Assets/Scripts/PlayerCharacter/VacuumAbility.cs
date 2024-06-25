using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerCharacter {
    public class VacuumAbility : MonoBehaviour {
        //PushBalls ability variables
        public float radius;

        [Range(0, 360)]
        public float angle;

        public LayerMask targetMask;

        [HideInInspector]
        public bool CanSeeVacuumableObject { get; private set; } = false;

        [HideInInspector]
        public Collider[] RayData { get; private set; }


        private void Start() {
            //Debug in editor
            StartCoroutine(FOVRoutine());
        }

        private IEnumerator FOVRoutine() {
            WaitForSeconds wait = new WaitForSeconds(0.1f);
            while (true) {
                yield return wait;
                FieldOfViewCheck();
            }
        }

        public void VacuumObjects() {
            // int maxColliders = 20;
            // Collider[] hitColliders = new Collider[maxColliders];
            // int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, targetMask);
            // for (int i = 0; i < numColliders; i++) {
            //     Transform target = hitColliders[i].transform;
            //     
            //     Vector3 directionToTarget = (target.position - transform.position).normalized;
            //
            //     // If Collider (target) is in the angle – Do something
            //     if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
            //         // TODO: Vacuuming the objects
            //     }
            // }

            

            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
            
            if (rangeChecks.Length != 0) {
                foreach (Collider colider in rangeChecks) {
                    Transform target = colider.transform;
            
                    Vector3 directionToTarget = (target.position - transform.position).normalized;
            
                    // If Collider (target) is in the angle – Do something
                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                        // TODO: Vacuuming the objects
                    }
                }
            }
        }

        public void FieldOfViewCheck() {
            // int maxColliders = 20;
            // Collider[] hitColliders = new Collider[maxColliders];
            // int numColliders = Physics.OverlapSphereNonAlloc(transform.position, radius, hitColliders, targetMask);
            //
            // if (numColliders != 0) {
            //     for (int i = 0; i < numColliders; i++) {
            //         Transform target = hitColliders[i].transform;
            //
            //         Vector3 directionToTarget = (target.position - transform.position).normalized;
            //
            //         if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
            //             CanSeeVacuumableObject = true;
            //         } else {
            //             CanSeeVacuumableObject = false;
            //         }
            //     }
            // } else if (CanSeeVacuumableObject) {
            //     CanSeeVacuumableObject = false;
            // }


            RayData = Physics.OverlapSphere(transform.position, radius, targetMask);
            
            if (RayData.Length != 0) {
                foreach (Collider colider in RayData) {
                    Transform target = colider.transform;
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