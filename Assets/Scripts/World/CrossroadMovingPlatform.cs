using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using EditorScripts.InvokeButton;
using KinematicCharacterController;
using QFSW.QC;
using UnityEngine;
using World.AbstractClasses;
using World.Enums;

namespace World {
    [RequireComponent(typeof(PhysicsMover))]
    public class CrossroadMovingPlatform : Interactable, IMoverController {
        [Space]
        public float speed = 2f;

        public Transform yellowPoint;
        public Transform redPoint;
        public Transform cyanPoint;
        public Transform greenPoint;
        public Transform bluePoint;
        public Transform pinkPoint;

        [Space]
        public PowerCoreColors startingPoint;
        public bool noPowerCoreLooping = false; 

        private PhysicsMover _mover;

        private Vector3 _startPosition;
        private Vector3 _targetPosition;
        private Vector3 _goalPosition;

        private float _journeyLength;
        private float _startTime;

        private bool _canMove = false;


        //Debug list of active points
        private List<Transform> _activePoints = new List<Transform>();

        private void Awake() {
            TryGetComponent<PhysicsMover>(out _mover);
        }

        void Start() {
            _mover.MoverController = this;

            _goalPosition = SelectNextPoint(startingPoint);
            _startPosition = SelectNextPoint(startingPoint);


            _targetPosition = SelectNextPoint(startingPoint);

            _canMove = false;

            _journeyLength = Vector3.Distance(_startPosition, _targetPosition);

            _startTime = Time.time;


            if (yellowPoint) {
                //_activePoints.Add(yellowPoint);
                yellowPoint?.SetParent(null);
            }
            if (redPoint) {
                //_activePoints.Add(redPoint);
                redPoint?.SetParent(null);
            }
            if (cyanPoint) {
                //_activePoints.Add(cyanPoint);
                cyanPoint?.SetParent(null);
            }
            if (greenPoint) {
                //_activePoints.Add(greenPoint);
                greenPoint?.SetParent(null);
            }
            if (bluePoint) {
                //_activePoints.Add(bluePoint);
                bluePoint?.SetParent(null);
            }
            if (pinkPoint) {
                //_activePoints.Add(pinkPoint);
                pinkPoint?.SetParent(null);
            }

        }

        [InvokeButton]
        public override void Interact() {

            if (_canMove) {
                ChangeDirectionMidJourney();
            } else {
                StartMovement();
            }
        }

        void Update() {
            if (_canMove) {
                MovePlatformSmoothly();
            }
        }

        private void MovePlatformSmoothly() {
            // Calculate the fraction of the journey completed
            float distCovered = (Time.time - _startTime) * speed;
            float fractionOfJourney = distCovered / _journeyLength;

            // Smooth step for easing movement
            float smoothStep = Mathf.SmoothStep(0, 1, fractionOfJourney);

            // Move the platform
            _goalPosition = Vector3.Lerp(_startPosition, _targetPosition, smoothStep);

            // Check if the platform has reached the target position
            if (fractionOfJourney >= 1f) {
                // Stop the platform
                _canMove = false;
            }
        }

        // Optional: If you want to change direction mid-movement, you can call this method anytime
        private void ChangeDirectionMidJourney() {
            var tempPos = SelectNextPoint(activatedBy);
            if (tempPos == _targetPosition) {
                return;
            }

            // Immediately swap direction, preserving the current position as the new start point
            _startPosition = transform.position;

            // Switch the target position
            _targetPosition = SelectNextPoint(activatedBy);


            // Recalculate the journey length and reset the timer
            _journeyLength = Vector3.Distance(_startPosition, _targetPosition);
            _startTime = Time.time;
        }

        private void StartMovement() {

            // Switch to move towards pointA
            _startPosition = _targetPosition;
            _targetPosition = SelectNextPoint(activatedBy);

            if (_startPosition == _targetPosition) {
                return;
            }

            // Reset the journey properties
            _journeyLength = Vector3.Distance(_startPosition, _targetPosition);
            _startTime = Time.time;

            // Start moving the platform
            _canMove = true;
        }


        // Draw gizmos to visualize points and lines between them
        void OnDrawGizmos() {
            
            if (yellowPoint) {
                _activePoints.Add(yellowPoint);
            }
            if (redPoint) {
                _activePoints.Add(redPoint);
            }
            if (cyanPoint) {
                _activePoints.Add(cyanPoint);
            }
            if (greenPoint) {
                _activePoints.Add(greenPoint);
            }
            if (bluePoint) {
                _activePoints.Add(bluePoint);
            }
            if (pinkPoint) {
                _activePoints.Add(pinkPoint);
            }

            if (yellowPoint) {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(yellowPoint.position, 0.5f);
            }
            if (redPoint) {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(redPoint.position, 0.5f);
            }
            if (cyanPoint) {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(cyanPoint.position, 0.5f);
            }
            if (greenPoint) {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(greenPoint.position, 0.5f);
            }
            if (bluePoint) {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(bluePoint.position, 0.5f);
            }
            if (pinkPoint) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(pinkPoint.position, 0.5f);
            }

            List<Vector3> points = new List<Vector3>();
            foreach (Transform activePoint in _activePoints) {
                points.Add(activePoint.position);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawLineStrip(points.ToArray(), true);

        }

        private Vector3 SelectNextPoint(PowerCoreColors targetPointColor) {
            switch (targetPointColor) {
                case PowerCoreColors.None:
                case PowerCoreColors.Any:
                    //Has no target, do not move
                    _canMove = false;
                    return transform.position;
                case PowerCoreColors.Yellow:
                    if (yellowPoint) {
                        return yellowPoint.position;
                    } else {
                        Debug.LogError("Crossroadplatform has not yellowPoint reference.");
                        _canMove = false;
                        return transform.position;
                    }
                case PowerCoreColors.Red:
                    if (redPoint) {
                        return redPoint.position;
                    } else {
                        Debug.LogError("Crossroadplatform has not redPoint reference.");
                        _canMove = false;
                        return transform.position;
                    }
                case PowerCoreColors.Cyan:
                    if (cyanPoint) {
                        return cyanPoint.position;
                    } else {
                        Debug.LogError("Crossroadplatform has not cyanPoint reference.");
                        _canMove = false;
                        return transform.position;
                    }
                case PowerCoreColors.Green:
                    if (greenPoint) {
                        return greenPoint.position;
                    } else {
                        Debug.LogError("Crossroadplatform has not greenPoint reference.");
                        _canMove = false;
                        return transform.position;
                    }
                case PowerCoreColors.Blue:
                    if (bluePoint) {
                        return bluePoint.position;
                    } else {
                        Debug.LogError("Crossroadplatform has not bluePoint reference.");
                        _canMove = false;
                        return transform.position;
                    }
                case PowerCoreColors.Pink:
                    if (pinkPoint) {
                        return pinkPoint.position;
                    } else {
                        Debug.LogError("Crossroadplatform has not pinkPoint reference.");
                        _canMove = false;
                        return transform.position;
                    }
                default:
                    Debug.LogError("Unknow color point");
                    _canMove = false;
                    return transform.position;
            }
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime) {
            goalPosition = _goalPosition;
            goalRotation = _mover.Rigidbody.rotation;
        }
    }
}