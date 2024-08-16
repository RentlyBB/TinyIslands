using UnityEngine;
using UnityEngine.Serialization;

namespace EditorScripts.Debug {
    public class PlatformWireVisualization : MonoBehaviour {
        
        public Transform objectToVisualizate;
        
        private void OnDrawGizmosSelected() {
            // Draw a yellow cube at the transform position

            if (objectToVisualizate == null) objectToVisualizate = transform;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, objectToVisualizate.localScale);
        }
    }
}