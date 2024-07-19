using System.Collections.Generic;
using InputCore;
using UnityEngine;
using World;

namespace PlayerCharacter {
    public class PlayerInteractController : MonoBehaviour {
        public InputReaderSo inputReader;

        public float sphereRadius = 2f;

        private void OnEnable() {
            inputReader.Interact += OnInteract;
        }

        private void OnDisable() {
            inputReader.Interact -= OnInteract;
        }

        private void OnDrawGizmosSelected() {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sphereRadius);
        }

        private void OnInteract() {
            int maxColliders = 10;
            Collider[] hitColliders = new Collider[maxColliders];

            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, sphereRadius, hitColliders);

            List<Interactable> interactableObjects = new List<Interactable>();

            for (int i = 0; i < numColliders; i++) {
                if (hitColliders[i] != null &&
                    hitColliders[i].TryGetComponent(out Interactable interactable)) {
                    interactableObjects.Add(interactable);
                }
            }

            Interactable closestInteractable = null;
            foreach (Interactable interactableObj in interactableObjects) {
                if (closestInteractable == null) {
                    closestInteractable = interactableObj;
                } else {
                    if (Vector3.Distance(transform.position, interactableObj.transform.position) <
                        Vector3.Distance(transform.position, closestInteractable.transform.position)) {
                        closestInteractable = interactableObj;
                    }
                }
            }

            closestInteractable?.InteractableAction();
        }
    }
}