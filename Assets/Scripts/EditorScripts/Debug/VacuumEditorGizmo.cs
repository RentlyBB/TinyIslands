using System.Collections;
using System.Collections.Generic;
using PlayerCharacter;
using UnityEditor;
using UnityEngine;

namespace EditorScripts.Debug {
    
    [CustomEditor(typeof(VacuumAbility))]
    public class VacuumEditorGizmo : Editor {
        private void OnSceneGUI() {
            
            VacuumAbility fov = (VacuumAbility)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

            Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
            Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

            
            // Draw a line between objects and player
            if(fov.CanSeeVacuumableObject) {
                Handles.color = Color.green;
                foreach(Collider colider in fov.RayData) {
                    Handles.DrawLine(fov.transform.position, colider.transform.position);
                }
            }
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}