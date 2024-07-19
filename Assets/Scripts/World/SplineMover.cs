using System;
using EditorScripts;
using UnityEngine;
using UnityEngine.Splines;

namespace World {
    public class SplineMover : MonoBehaviour {
        [SerializeField]
        private Transform track;

        [Header("Starting position on track")]
        [SerializeField]
        [Range(0f, 1f)]
        private float railCartPosition = 0.5f;

        public bool rotateOnSpline = false;

        private SplineContainer _trackSplineContainer;

        private Spline _trackSpline;

        private void Start() {
            if (track != null) {
                _trackSplineContainer = track.GetComponent<SplineContainer>();
                _trackSpline = _trackSplineContainer.Spline;
            } else {
                Debug.LogError(this.name + " has not assign spline track.");
            }

            // Set starting position for player
            this.transform.position = track.TransformPoint(_trackSpline.EvaluatePosition(railCartPosition));
        }

        private void Update() {
            MoveCartOnSpline();
            if (rotateOnSpline) {
                RotateCartOnSpline();
            }
        }

        public void MoveCartOnSpline() {
            railCartPosition = RoundToThreeDigit(railCartPosition);

            // Update current player position with new one
            this.transform.position = track.TransformPoint(_trackSpline.EvaluatePosition(railCartPosition));

            // In case of rewriting movement to physics based, this is the world position/direction
            // where force should face >> track.TransformDirection(trackSpline.EvaluatePosition(railCartPosition))
        }

        private void RotateCartOnSpline() {
            // Calculate up vector of the spline
            var upSplineDirection = SplineUtility.EvaluateUpVector(_trackSpline, railCartPosition);

            Vector3 direction = SplineUtility.EvaluateTangent(_trackSpline, railCartPosition);
            Vector3 worldDirection = track.TransformDirection(direction);

            Vector3 targetRotation = Quaternion.Euler(0, -90f, 0) * worldDirection;

            if (targetRotation != Vector3.zero) {
                this.transform.rotation = Quaternion.LookRotation(targetRotation, upSplineDirection);
            }
        }

        private float RoundToThreeDigit(float val) {
            return (float)Math.Round(val, 4);
        }
    }
}