using UnityEngine;

namespace KinematicCharacterController.Walkthrough.ClimbingLadders {
    public class MyLadder : MonoBehaviour {
        // Ladder segment
        public Vector3 LadderSegmentBottom;
        public float LadderSegmentLength;

        // Points to move to when reaching one of the extremities and moving off of the ladder
        public Transform BottomReleasePoint;
        public Transform TopReleasePoint;

        // Gets the position of the bottom point of the ladder segment
        public Vector3 BottomAnchorPoint => transform.position + transform.TransformVector(LadderSegmentBottom);

        // Gets the position of the top point of the ladder segment
        public Vector3 TopAnchorPoint => transform.position + transform.TransformVector(LadderSegmentBottom) + transform.up * LadderSegmentLength;

        private void OnDrawGizmos() {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(BottomAnchorPoint, TopAnchorPoint);
        }

        public Vector3 ClosestPointOnLadderSegment(Vector3 fromPoint, out float onSegmentState) {
            var segment = TopAnchorPoint - BottomAnchorPoint;
            var segmentPoint1ToPoint = fromPoint - BottomAnchorPoint;
            var pointProjectionLength = Vector3.Dot(segmentPoint1ToPoint, segment.normalized);

            // When higher than bottom point
            if (pointProjectionLength > 0) {
                // If we are not higher than top point
                if (pointProjectionLength <= segment.magnitude) {
                    onSegmentState = 0;
                    return BottomAnchorPoint + segment.normalized * pointProjectionLength;
                }

                // If we are higher than top point
                onSegmentState = pointProjectionLength - segment.magnitude;
                return TopAnchorPoint;
            }

            // When lower than bottom point
            onSegmentState = pointProjectionLength;
            return BottomAnchorPoint;
        }
    }
}