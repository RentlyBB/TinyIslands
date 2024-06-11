using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace EditorScripts {
    [CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
    public class InvokeButtonEditor : Editor {
        
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            // Ensure multi-object editing is not used
            if (serializedObject.isEditingMultipleObjects) {
                EditorGUILayout.HelpBox("InvokeButton does not support multi-object editing.", MessageType.Warning);
                return;
            }

            // Get the target object
            var targetObject = target as MonoBehaviour;

            // Get all methods in the target object
            var methods = targetObject?.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            // Iterate through the methods
            if (methods != null) {
                foreach (var method in methods) {
                    // Check if the method has the InvokeButton attribute
                    var attribute = method.GetCustomAttribute<InvokeButtonAttribute>();
                    if (attribute != null) {
                        // Determine the button label
                        string buttonLabel = attribute.ButtonName ?? method.Name;

                        // Create the button
                        if (GUILayout.Button(buttonLabel)) {
                            // Invoke the method when the button is clicked
                            method.Invoke(targetObject, null);
                        }
                    }
                }
            }
        }
    }
}