using UnityEditor;

namespace ScriptableObjectArchitecture.Editor {
    public abstract class BaseGameEventEditor : UnityEditor.Editor {
        private StackTrace _stackTrace;
        private IStackTraceObject Target => (IStackTraceObject)target;

        protected virtual void OnEnable() {
            _stackTrace = new StackTrace(Target);
            _stackTrace.OnRepaint.AddListener(Repaint);
        }

        protected abstract void DrawRaiseButton();

        public override void OnInspectorGUI() {
            DrawRaiseButton();

            if (!SOArchitecturePreferences.IsDebugEnabled)
                EditorGUILayout.HelpBox("Debug mode disabled\nStack traces will not be filed on raise!", MessageType.Warning);

            _stackTrace.Draw();
        }
    }
}