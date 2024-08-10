using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ScriptableObjectArchitecture {
    public class SOArchitecture_Settings : ScriptableObject {
        [SerializeField]
        private string _codeGenerationTargetDirectory = "CODE_GENERATION";

        [SerializeField]
        [Tooltip("Allow newly generated code files to overwrite existing ones")]
        private bool _codeGenerationAllowOverwrite;

        [SerializeField]
        private int _defualtCreateAssetMenuOrder = 120;

        public string CodeGenerationTargetDirectory {
            get => _codeGenerationTargetDirectory;
            set => _codeGenerationTargetDirectory = value;
        }

        public bool CodeGenerationAllowOverwrite {
            get => _codeGenerationAllowOverwrite;
            set => _codeGenerationAllowOverwrite = value;
        }

        public int DefaultCreateAssetMenuOrder {
            get => _defualtCreateAssetMenuOrder;
            set => _defualtCreateAssetMenuOrder = value;
        }

        #region Singleton

        public static SOArchitecture_Settings Instance {
            get {
                if (_instance == null)
                    _instance = GetInstance();

                return _instance;
            }
        }

        private static SOArchitecture_Settings _instance;

        private static SOArchitecture_Settings GetInstance() {
#if UNITY_EDITOR
            var instance = FindInstanceInProject();

            if (instance == null)
                return CreateInstance();

            return instance;
#else
            return null;
#endif
        }

        private static SOArchitecture_Settings FindInstanceInProject() {
#if UNITY_EDITOR
            var settingsGUIDs = AssetDatabase.FindAssets(AssetDatabaseSearchString);

            if (settingsGUIDs.Length == 0) return null;
            if (settingsGUIDs.Length > 1) {
                Debug.LogWarning("Found more than one instance of SOArchitecture_Settings, you've probably created several SOA settings objects." +
                                 $"\nTo find all instances, type {AssetDatabaseSearchString} into the project view search bar");
                return null;
            }

            var settingsPath = AssetDatabase.GUIDToAssetPath(settingsGUIDs[0]);
            return AssetDatabase.LoadAssetAtPath<SOArchitecture_Settings>(settingsPath);
#else
            throw new System.NullReferenceException();
#endif
        }

        private static SOArchitecture_Settings CreateInstance() {
#if UNITY_EDITOR
            var newSettings = CreateInstance<SOArchitecture_Settings>();

            AssetDatabase.CreateAsset(newSettings, DefaultNewSettingsLocation + DefaultNewSettingsName);
            AssetDatabase.SaveAssets();

            Selection.activeObject = newSettings;

            Debug.LogWarning("No SOArchitecture_Settings asset found! " +
                             "Created new one at asset root, feel free to move it wherever you please in your project.", newSettings);

            return newSettings;
#else
        throw new System.NullReferenceException();
#endif
        }

        private const string AssetDatabaseSearchString = "t:SOArchitecture_Settings";
        private const string DefaultNewSettingsLocation = "Assets\\";
        private const string DefaultNewSettingsName = "SOArchitecture_Settings.asset";

        #endregion
    }
}