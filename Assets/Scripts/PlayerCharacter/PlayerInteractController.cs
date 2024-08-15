using System.Collections.Generic;
using InputCore;
using Systems;
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
            var maxColliders = 10;
            var hitColliders = new Collider[maxColliders];

            var numColliders = Physics.OverlapSphereNonAlloc(transform.position, sphereRadius, hitColliders);

            var interactableObjects = new List<Activator>();

            for (var i = 0; i < numColliders; i++)
                if (hitColliders[i] != null &&
                    hitColliders[i].TryGetComponent(out Activator interactable))
                    interactableObjects.Add(interactable);

            Activator closestActivator = null;
            foreach (var interactableObj in interactableObjects)
                if (closestActivator == null) {
                    closestActivator = interactableObj;
                } else {
                    if (Vector3.Distance(transform.position, interactableObj.transform.position) <
                        Vector3.Distance(transform.position, closestActivator.transform.position))
                        closestActivator = interactableObj;
                }

            closestActivator?.InteractableAction();
        }
    }
}