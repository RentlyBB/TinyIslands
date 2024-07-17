using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor {
    [CustomEditor(typeof(BaseVariable<>), true)]
    public class BaseVariableEditor : UnityEditor.Editor {

        private const string READONLY_TOOLTIP = "Should this value be changable during runtime? Will still be editable in the inspector regardless";
        private SerializedProperty _isClamped;
        private AnimBool _isClampedVariableAnimation;
        private SerializedProperty _maxValueProperty;
        private SerializedProperty _minValueProperty;
        private SerializedProperty _raiseWarning;
        private AnimBool _raiseWarningAnimation;
        private SerializedProperty _readOnly;

        private SerializedProperty _valueProperty;
        private BaseVariable Target { get { return (BaseVariable)target; } }
        protected bool IsClampable { get { return Target.Clampable; } }
        protected bool IsClamped { get { return Target.IsClamped; } }

        protected virtual void OnEnable() {
            _valueProperty = serializedObject.FindProperty("_value");
            _readOnly = serializedObject.FindProperty("_readOnly");
            _raiseWarning = serializedObject.FindProperty("_raiseWarning");
            _isClamped = serializedObject.FindProperty("_isClamped");
            _minValueProperty = serializedObject.FindProperty("_minClampedValue");
            _maxValueProperty = serializedObject.FindProperty("_maxClampedValue");

            _raiseWarningAnimation = new AnimBool(_readOnly.boolValue);
            _raiseWarningAnimation.valueChanged.AddListener(Repaint);

            _isClampedVariableAnimation = new AnimBool(_isClamped.boolValue);
            _isClampedVariableAnimation.valueChanged.AddListener(Repaint);
        }
        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawValue();

            EditorGUILayout.Space();

            DrawClampedFields();
            DrawReadonlyField();
        }
        protected virtual void DrawValue() {
            GenericPropertyDrawer.DrawPropertyDrawerLayout(_valueProperty, Target.Type);
        }
        protected void DrawClampedFields() {
            if (!IsClampable)
                return;

            EditorGUILayout.PropertyField(_isClamped);
            _isClampedVariableAnimation.target = _isClamped.boolValue;

            using (EditorGUILayout.FadeGroupScope anim = new EditorGUILayout.FadeGroupScope(_isClampedVariableAnimation.faded)) {
                if (anim.visible) {
                    using (new EditorGUI.IndentLevelScope()) {
                        EditorGUILayout.PropertyField(_minValueProperty);
                        EditorGUILayout.PropertyField(_maxValueProperty);
                    }
                }
            }

        }
        protected void DrawReadonlyField() {
            if (IsClampable)
                return;

            EditorGUILayout.PropertyField(_readOnly, new GUIContent("Read Only", READONLY_TOOLTIP));

            _raiseWarningAnimation.target = _readOnly.boolValue;
            using (EditorGUILayout.FadeGroupScope fadeGroup = new EditorGUILayout.FadeGroupScope(_raiseWarningAnimation.faded)) {
                if (fadeGroup.visible) {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_raiseWarning);
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
    [CustomEditor(typeof(BaseVariable<,>), true)]
    public class BaseVariableWithEventEditor : BaseVariableEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_event"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}