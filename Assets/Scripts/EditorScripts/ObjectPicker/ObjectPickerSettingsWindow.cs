using UnityEngine;
using UnityEditor;

namespace EditorScripts {
    public class ObjectPickerSettingsWindow : EditorWindow {
        private ObjectPickerSettings _settings;

        [MenuItem("Tools/Object Picker Settings")]
        public static void ShowWindow() {
            GetWindow<ObjectPickerSettingsWindow>("Object Picker Settings");
        }

        private void OnEnable() {
            // Load or create the settings asset
            _settings = AssetDatabase.LoadAssetAtPath<ObjectPickerSettings>("Assets/Resources/ObjectPickerSettings.asset");
            if (_settings == null) {
                _settings = CreateInstance<ObjectPickerSettings>();
                AssetDatabase.CreateAsset(_settings, "Assets/Resources/ObjectPickerSettings.asset");
                AssetDatabase.SaveAssets();
            }
        }

        private void OnGUI() {
            if (!_settings) {
                EditorGUILayout.HelpBox("Settings not found or created.", MessageType.Error);
                return;
            }

            EditorGUILayout.LabelField("Ignore Settings", EditorStyles.boldLabel);

            // Display the tag ignore list
            SerializedObject serializedSettings = new SerializedObject(_settings);
            SerializedProperty ignoreTagsProp = serializedSettings.FindProperty("ignoreTags");

            EditorGUILayout.PropertyField(ignoreTagsProp, new GUIContent("Ignore Tags"), true);

            // Use the custom LayerMaskField for selecting layers
            _settings.ignoreLayers = CustomEditorGUILayout.LayerMaskField("Ignore Layers", _settings.ignoreLayers);

            serializedSettings.ApplyModifiedProperties();

            if (GUILayout.Button("Save Settings")) {
                EditorUtility.SetDirty(_settings);
                AssetDatabase.SaveAssets();
            }
        }
    }
}