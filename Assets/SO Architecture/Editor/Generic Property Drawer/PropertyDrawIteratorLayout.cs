using UnityEditor;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor {
    public class PropertyDrawIteratorLayout : BasePropertyDrawIterator {
        public PropertyDrawIteratorLayout(SerializedProperty property, bool drawLabel) : base(property, drawLabel) {
        }

        protected override void DrawPropertyWithLabel() {
            EditorGUILayout.PropertyField(iterator);
        }
        protected override void DrawProperty() {
            EditorGUILayout.PropertyField(iterator, GUIContent.none);
        }
    }
}