using UnityEditor;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor {
    public class PropertyDrawIterator : BasePropertyDrawIterator {

        private Rect rect;
        public PropertyDrawIterator(Rect rect, SerializedProperty property, bool drawLabel) : base(property, drawLabel) {
            this.rect = rect;
            this.rect.height = EditorGUIUtility.singleLineHeight;
        }

        public override void Draw() {
            base.Draw();

            MoveRectDownOneLine();
        }

        protected override void DrawPropertyWithLabel() {
            EditorGUI.PropertyField(rect, iterator);
        }
        protected override void DrawProperty() {
            EditorGUI.PropertyField(rect, iterator, GUIContent.none);
        }

        private void MoveRectDownOneLine() {
            rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}