using UnityEditor;
using UnityEngine;

namespace EditorScripts {
    [CustomPropertyDrawer(typeof(Object), true)]
    public class GlobalObjectPickerDrawer : PropertyDrawer {
        private bool isPicking = false;
        private SerializedProperty currentProperty;
        private ObjectPickerSettings settings;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // Load settings if not already loaded
            if (settings == null) {
                settings = AssetDatabase.LoadAssetAtPath<ObjectPickerSettings>("Assets/ObjectPickerSettings.asset");
            }

            // Calculate rects for the object field and button
            Rect fieldRect = new Rect(position.x, position.y, position.width - 30, position.height);
            Rect buttonRect = new Rect(position.x + position.width - 25, position.y, 25, position.height);

            // Draw the object field
            EditorGUI.PropertyField(fieldRect, property, label);

            // Draw the "Pick" button
            if (GUI.Button(buttonRect, "Pick")) {
                isPicking = !isPicking;

                if (isPicking) {
                    currentProperty = property;
                    SceneView.duringSceneGui += OnSceneGUI;
                } else {
                    SceneView.duringSceneGui -= OnSceneGUI;
                }
            }
        }

        private void OnSceneGUI(SceneView sceneView) {
            if (isPicking && Event.current.type == EventType.MouseDown && Event.current.button == 0) {
                // Raycast to find the clicked object in the scene
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    GameObject clickedObject = hit.collider.gameObject;

                    // Check if the object should be ignored based on global settings
                    if (ShouldIgnoreObject(clickedObject)) {
                        //Debug.Log("Object ignored due to global settings.");
                    } else {
                        // Assign the clicked object to the property
                        currentProperty.objectReferenceValue = clickedObject;
                        currentProperty.serializedObject.ApplyModifiedProperties();

                        // Exit picking mode
                        isPicking = false;
                        SceneView.duringSceneGui -= OnSceneGUI;

                        // Repaint the inspector to update the UI
                        EditorWindow.focusedWindow.Repaint();
                    }
                }

                Event.current.Use();
            }
        }

        private bool ShouldIgnoreObject(GameObject obj) {
            if (settings == null)
                return false;

            // Check if the object's tag is in the ignore list
            foreach (string tag in settings.ignoreTags) {
                if (obj.CompareTag(tag))
                    return true;
            }

            // Check if the object's layer is in the ignore list
            if ((settings.ignoreLayers.value & (1 << obj.layer)) != 0) {
                return true;
            }

            return false;
        }
    }
}