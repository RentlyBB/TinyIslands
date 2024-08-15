using UnityEditor;
using UnityEngine;

namespace EditorScripts.HideIf {
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // Get the HideIf attribute
            HideIfAttribute hideIf = (HideIfAttribute)attribute;

            // Get the boolean field value that determines if the property should be hidden
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(hideIf.ConditionalSourceField);

            if (sourcePropertyValue != null && sourcePropertyValue.propertyType == SerializedPropertyType.Boolean) {
                // If the condition is met (the boolean is false), draw the property
                if (!sourcePropertyValue.boolValue) {
                    EditorGUI.PropertyField(position, property, label, true);
                }
            } else {
                // If the source property is not found or isn't a boolean, display a warning and the property
                UnityEngine.Debug.LogWarning("HideIf: Property is not a boolean or could not be found.");
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            HideIfAttribute hideIf = (HideIfAttribute)attribute;
            SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(hideIf.ConditionalSourceField);

            if (sourcePropertyValue != null && sourcePropertyValue.propertyType == SerializedPropertyType.Boolean) {
                if (!sourcePropertyValue.boolValue) {
                    return EditorGUI.GetPropertyHeight(property, label);
                } else {
                    // Return 0 height when the property is hidden
                    return -EditorGUIUtility.standardVerticalSpacing;
                }
            } else {
                return EditorGUI.GetPropertyHeight(property, label);
            }
        }
    }
}