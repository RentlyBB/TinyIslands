using System.Collections.Generic;
using KinematicCharacterController;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerCharacter.Abilities {
    public class FallPreventer : MonoBehaviour {
        public Transform fallWallPoints;
        public Transform fallWalls;
        public float fallThreshold = 2f;

        public LayerMask ignoreLayerMasks = 1 << 8;
        private readonly List<Transform> _fallWalls = new List<Transform>();
        private RaycastHit _hit;

        private KinematicCharacterMotor _motor;

        private readonly List<Transform> _wallPoints = new List<Transform>();

        private void Start() {
            _motor = GetComponent<KinematicCharacterMotor>();
            //ignoreLayerMasks = ~ignoreLayerMasks;

            fallWallPoints.position = transform.position;
            fallWalls.position = transform.position;

            foreach (Transform wallPoint in fallWallPoints) {
                _wallPoints.Add(wallPoint);
            }

            foreach (Transform fallWall in fallWalls) {
                _fallWalls.Add(fallWall);
            }
        }

        private void FixedUpdate() {
            SetWallPosition();
        }

        private void SetWallPosition() {
            fallWallPoints.position = transform.position;

            //if (!_motor.GroundingStatus.GroundCollider) return;

            for (int i = 0; i < _wallPoints.Count; i++) {
                fallWalls.position = transform.position;

                if (!Physics.Raycast(_wallPoints[i].position, _wallPoints[i].TransformDirection(Vector3.down), out _hit, fallThreshold, ~ignoreLayerMasks) && _motor.GroundingStatus.GroundCollider) {
                    Vector3 closestPoint = _motor.GroundingStatus.GroundCollider.ClosestPointOnBounds(_wallPoints[i].position);
                    Vector3 targetWallPos = new Vector3(closestPoint.x, transform.position.y + 1, closestPoint.z);
                    _fallWalls[i].position = targetWallPos;

                    Debug.DrawRay(_wallPoints[i].position, _wallPoints[i].TransformDirection(Vector3.down) * fallThreshold, Color.yellow);

                } else {
                    _fallWalls[i].position = _wallPoints[i].position;

                    Debug.DrawRay(_wallPoints[i].position, _wallPoints[i].TransformDirection(Vector3.down) * fallThreshold, Color.red);
                }
            }
        }
    }
}