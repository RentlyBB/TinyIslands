using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture {
    [Serializable]
    public class SceneInfoEvent : UnityEvent<SceneInfo> { }

    /// <summary>
    ///     <see cref="SceneVariable" /> is a scriptable constant variable whose scene values are assigned at
    ///     edit-time by assigning a <see cref="UnityEditor.SceneAsset" /> instance to it.
    /// </summary>
    [CreateAssetMenu(
        fileName = "SceneVariable.asset",
        menuName = SOArchitecture_Utility.ADVANCED_VARIABLE_SUBMENU + "Scene",
        order = 120)]
    public sealed class SceneVariable : BaseVariable<SceneInfo, SceneInfoEvent> {
        /// <summary>
        ///     Returns the <see cref="SceneInfo" /> of this instance.
        /// </summary>
        public override SceneInfo Value => _value;

        public override bool ReadOnly =>
            // A scene variable is essentially a constant for edit-time modification only; there is not
            // any kind of expectation for a user to be able to set this at runtime.
            true;
    }

    [Serializable]
    [MultiLine]
    public sealed class SceneInfo : ISerializationCallbackReceiver {
        public SceneInfo() {
            _sceneIndex = -1;
        }

        /// <summary>
        ///     Returns the fully-qualified name of the scene.
        /// </summary>
        public string SceneName => _sceneName;

        /// <summary>
        ///     Returns the index of the scene in the build settings; if not present, -1 will be returned instead.
        /// </summary>
        public int SceneIndex {
            get => _sceneIndex;
            internal set => _sceneIndex = value;
        }

        /// <summary>
        ///     Returns true if the scene is present in the build settings, otherwise false.
        /// </summary>
        public bool IsSceneInBuildSettings => _sceneIndex != -1;

        /// <summary>
        ///     Returns true if the scene is enabled in the build settings, otherwise false.
        /// </summary>
        public bool IsSceneEnabled {
            get => _isSceneEnabled;
            internal set => _isSceneEnabled = value;
        }

#if UNITY_EDITOR
        internal SceneAsset Scene => AssetDatabase.LoadAssetAtPath<SceneAsset>(_sceneName);
#endif

#pragma warning disable 0649

        [SerializeField]
        private string _sceneName;

        [SerializeField]
        private int _sceneIndex;

        [SerializeField]
        private bool _isSceneEnabled;

#pragma warning restore 0649

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize() {
#if UNITY_EDITOR
            if (Scene != null) {
                var sceneAssetPath = AssetDatabase.GetAssetPath(Scene);
                var sceneAssetGUID = AssetDatabase.AssetPathToGUID(sceneAssetPath);
                var scenes = EditorBuildSettings.scenes;

                SceneIndex = -1;
                for (var i = 0; i < scenes.Length; i++)
                    if (scenes[i].guid.ToString() == sceneAssetGUID) {
                        SceneIndex = i;
                        IsSceneEnabled = scenes[i].enabled;
                        break;
                    }
            }
#endif
        }

        public void OnAfterDeserialize() { }

        #endregion
    }
}