using UnityEngine;
using World;

namespace PlayerCharacter {
    public class VacuumAbility : MonoBehaviour {

        public float radius;

        [Range(0, 360)]
        public float angle;

        public LayerMask targetMask;

        public Collider[] RayData { get; }

        private void Update() {
            VacuumObjects();
        }

        public void VacuumObjects() {
            Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

            if (rangeChecks.Length != 0) {
                foreach (Collider coll in rangeChecks) {
                    Transform target = coll.transform;

                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    // Collider (target) is in the angle – Do something
                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                        // TODO: Vacuuming the objects

                        // TODO: This should be made by object pooling 
                        target.GetComponent<TrashBehaviour>().TrashActivate(transform.position);
                        Debug.DrawLine(transform.position, target.position, Color.green);
                    }
                }
            }
        }
    }
}