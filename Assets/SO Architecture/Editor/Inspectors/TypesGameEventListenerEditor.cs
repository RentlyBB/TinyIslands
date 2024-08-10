using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor {
    [CustomEditor(typeof(BaseGameEventListener<,,>), true)]
    public class TypesGameEventListenerEditor : BaseGameEventListenerEditor {
        private MethodInfo _raiseMethod;

        protected override void OnEnable() {
            base.OnEnable();

            _raiseMethod = target.GetType().BaseType.GetMethod("OnEventRaised");
        }

        protected override void DrawRaiseButton() {
            var property = serializedObject.FindProperty("_debugValue");

            EditorGUILayout.PropertyField(property);

            if (GUILayout.Button("Raise")) CallMethod(GetDebugValue(property));
        }

        private object GetDebugValue(SerializedProperty property) {
            var targetType = property.serializedObject.targetObject.GetType();
            var targetField = targetType.GetField("_debugValue", BindingFlags.Instance | BindingFlags.NonPublic);

            return targetField.GetValue(property.serializedObject.targetObject);
        }

        private void CallMethod(object value) {
            _raiseMethod.Invoke(target, new object[1] { value });
        }
    }
}