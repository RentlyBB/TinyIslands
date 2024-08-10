using PlayerCharacter.Abilities;
using UnityEditor;
using UnityEngine;

namespace EditorScripts.Debug {
    [CustomEditor(typeof(VacuumAbility))]
    public class VacuumEditorGizmo : Editor {
        private void OnSceneGUI() {
            var fov = (VacuumAbility)target;
            Handles.color = Color.white;
            Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

            var viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
            var viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
            Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees) {
            angleInDegrees += eulerY;

            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}