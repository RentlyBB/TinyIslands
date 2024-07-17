using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor {
    [CustomEditor(typeof(BaseCollection), true)]
    public class CollectionEditor : UnityEditor.Editor {

        // UI
        private const bool DISABLE_ELEMENTS = false;
        private const bool ELEMENT_DRAGGABLE = true;
        private const bool LIST_DISPLAY_HEADER = true;
        private const bool LIST_DISPLAY_ADD_BUTTON = true;
        private const bool LIST_DISPLAY_REMOVE_BUTTON = true;

        private const string TITLE_FORMAT = "List ({0})";
        private const string NO_PROPERTY_WARNING_FORMAT = "No PropertyDrawer for type [{0}]";

        // Property Names
        private const string LIST_PROPERTY_NAME = "_list";
        private GUIContent _noPropertyDrawerWarningGUIContent;

        private ReorderableList _reorderableList;

        private GUIContent _titleGUIContent;
        private BaseCollection Target { get { return (BaseCollection)target; } }
        private SerializedProperty CollectionItemsProperty {
            get { return serializedObject.FindProperty(LIST_PROPERTY_NAME); }
        }

        private void OnEnable() {
            _titleGUIContent = new GUIContent(string.Format(TITLE_FORMAT, Target.Type));
            _noPropertyDrawerWarningGUIContent = new GUIContent(string.Format(NO_PROPERTY_WARNING_FORMAT, Target.Type));

            _reorderableList = new ReorderableList(
                serializedObject,
                CollectionItemsProperty,
                ELEMENT_DRAGGABLE,
                LIST_DISPLAY_HEADER,
                LIST_DISPLAY_ADD_BUTTON,
                LIST_DISPLAY_REMOVE_BUTTON) {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement,
                elementHeightCallback = GetHeight,
            };
        }
        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();

            _reorderableList.DoLayoutList();

            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }
        }
        private void DrawHeader(Rect rect) {
            EditorGUI.LabelField(rect, _titleGUIContent);
        }
        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            rect = SOArchitecture_EditorUtility.GetReorderableListElementFieldRect(rect);
            SerializedProperty property = CollectionItemsProperty.GetArrayElementAtIndex(index);

            EditorGUI.BeginDisabledGroup(DISABLE_ELEMENTS);

            GenericPropertyDrawer.DrawPropertyDrawer(rect, property, Target.Type);

            EditorGUI.EndDisabledGroup();
        }
        private float GetHeight(int index) {
            SerializedProperty property = CollectionItemsProperty.GetArrayElementAtIndex(index);

            return GenericPropertyDrawer.GetHeight(property, Target.Type) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}