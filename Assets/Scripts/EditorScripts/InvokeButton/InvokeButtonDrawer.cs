using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorScripts.InvokeButton {
    [CustomPropertyDrawer(typeof(InvokeButtonAttribute))]
    public class InvokeButtonDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // Retrieve the InvokeButtonAttribute
            var invokeButton = attribute as InvokeButtonAttribute;

            // Get the target object
            var targetObject = property.serializedObject.targetObject;
            var targetType = targetObject.GetType();

            // Find the method with the InvokeButton attribute
            var method = targetType.GetMethod(property.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (method != null) {
                // Determine the button label
                var buttonLabel = invokeButton?.ButtonName ?? property.name;

                // Create the button
                if (GUI.Button(position, buttonLabel))
                    // Invoke the method when the button is clicked
                    method.Invoke(targetObject, null);
            } else {
                EditorGUI.LabelField(position, label.text, "Method not found or is inaccessible");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}