using UnityEditor;

namespace ScriptableObjectArchitecture.Editor {
    public abstract class BasePropertyDrawIterator : PropertyIterator, IPropertyDrawIterator {

        protected readonly bool drawLabel;
        protected readonly int startDepth;
        protected readonly int startIndentLevel;
        public BasePropertyDrawIterator(SerializedProperty property, bool drawLabel) : base(property) {
            this.drawLabel = drawLabel;
            startIndentLevel = EditorGUI.indentLevel;
            startDepth = iterator.depth;
        }

        public virtual void Draw() {
            EditorGUI.indentLevel = GetIndent(iterator.depth);

            if (IsCustom(iterator)) {
                if (drawLabel) {
                    DrawPropertyWithLabel();
                } else {
                    DrawProperty();
                }
            } else {
                if (drawLabel) {
                    DrawPropertyWithLabel();
                } else {
                    DrawProperty();
                }
            }
        }
        public override void End() {
            base.End();

            EditorGUI.indentLevel = startIndentLevel;
        }

        protected abstract void DrawProperty();
        protected abstract void DrawPropertyWithLabel();
        private int GetIndent(int depth) {
            return startIndentLevel + (depth - startDepth);
        }
        private bool IsCustom(SerializedProperty property) {
            return property.propertyType == SerializedPropertyType.Generic;
        }
    }
}