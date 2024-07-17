using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EditorScripts {
    [CustomPropertyDrawer(typeof(InvokeButtonAttribute))]
    public class InvokeButtonDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // Retrieve the InvokeButtonAttribute
            InvokeButtonAttribute invokeButton = attribute as InvokeButtonAttribute;

            // Get the target object
            Object targetObject = property.serializedObject.targetObject;
            Type targetType = targetObject.GetType();

            // Find the method with the InvokeButton attribute
            MethodInfo method = targetType.GetMethod(property.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (method != null) {
                // Determine the button label
                string buttonLabel = invokeButton?.ButtonName ?? property.name;

                // Create the button
                if (GUI.Button(position, buttonLabel)) {
                    // Invoke the method when the button is clicked
                    method.Invoke(targetObject, null);
                }
            } else {
                EditorGUI.LabelField(position, label.text, "Method not found or is inaccessible");
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}