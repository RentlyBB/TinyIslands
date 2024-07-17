﻿using System.Collections.Generic;
using UnityEngine;

namespace KinematicCharacterController.Examples {
    public class PlanetManager : MonoBehaviour, IMoverController {
        public PhysicsMover PlanetMover;
        public SphereCollider GravityField;
        public float GravityStrength = 10;
        public Vector3 OrbitAxis = Vector3.forward;
        public float OrbitSpeed = 10;

        public Teleporter OnPlaygroundTeleportingZone;
        public Teleporter OnPlanetTeleportingZone;

        private readonly List<ExampleCharacterController> _characterControllersOnPlanet = new List<ExampleCharacterController>();
        private Quaternion _lastRotation;
        private Vector3 _savedGravity;

        private void Start() {
            OnPlaygroundTeleportingZone.OnCharacterTeleport -= ControlGravity;
            OnPlaygroundTeleportingZone.OnCharacterTeleport += ControlGravity;

            OnPlanetTeleportingZone.OnCharacterTeleport -= UnControlGravity;
            OnPlanetTeleportingZone.OnCharacterTeleport += UnControlGravity;

            _lastRotation = PlanetMover.transform.rotation;

            PlanetMover.MoverController = this;
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = PlanetMover.Rigidbody.position;

            // Rotate
            Quaternion targetRotation = Quaternion.Euler(OrbitAxis * OrbitSpeed * deltaTime) * _lastRotation;
            goalRotation = targetRotation;
            _lastRotation = targetRotation;

            // Apply gravity to characters
            foreach (ExampleCharacterController cc in _characterControllersOnPlanet) {
                cc.Gravity = (PlanetMover.transform.position - cc.transform.position).normalized * GravityStrength;
            }
        }

        private void ControlGravity(ExampleCharacterController cc) {
            _savedGravity = cc.Gravity;
            _characterControllersOnPlanet.Add(cc);
        }

        private void UnControlGravity(ExampleCharacterController cc) {
            cc.Gravity = _savedGravity;
            _characterControllersOnPlanet.Remove(cc);
        }
    }
}