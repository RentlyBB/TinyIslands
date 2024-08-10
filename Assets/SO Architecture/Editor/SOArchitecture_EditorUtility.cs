using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Type = System.Type;

namespace ScriptableObjectArchitecture.Editor {
    public static class SOArchitecture_EditorUtility {
        private const float DebugStyleBackgroundAlpha = 0.33f;

        private static PropertyDrawerGraph _propertyDrawerGraph;
        private static readonly BindingFlags _fieldBindingsFlag = BindingFlags.Instance | BindingFlags.NonPublic;

        static SOArchitecture_EditorUtility() {
            CreateDebugStyle();
        }

        /// <summary>
        ///     A debug <see cref="GUIStyle" /> that allows for identification of EditorGUI Rect issues.
        /// </summary>
        public static GUIStyle DebugStyle { get; private set; }

        private static void CreatePropertyDrawerGraph() {
            _propertyDrawerGraph = new PropertyDrawerGraph();
            var assemblyNamesToCheck = new HashSet<string> {
                "Assembly-CSharp-Editor"
            };

            GetAllAssetDefintionNames(assemblyNamesToCheck);

            var dataPath = Application.dataPath;
            var libraryPath = dataPath.Substring(0, dataPath.LastIndexOf('/')) + "/Library/ScriptAssemblies";

            foreach (var file in Directory.GetFiles(libraryPath))
                if (assemblyNamesToCheck.Contains(Path.GetFileNameWithoutExtension(file)) && Path.GetExtension(file) == ".dll") {
                    var assembly = Assembly.LoadFrom(file);
                    _propertyDrawerGraph.CreateGraph(assembly);
                }
        }

        private static void GetAllAssetDefintionNames(HashSet<string> targetList) {
            var assemblyDefinitionGUIDs = AssetDatabase.FindAssets("t:asmdef");

            foreach (var guid in assemblyDefinitionGUIDs) {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                if (path.StartsWith("Assets/")) {
                    var fullPath = Application.dataPath + path.Remove(0, path.IndexOf('/'));

                    targetList.Add(GetNameValueFromAssemblyDefinition(fullPath));
                }
            }
        }

        private static string GetNameValueFromAssemblyDefinition(string fullpath) {
            var allText = File.ReadAllText(fullpath);
            var surrogate = JsonUtility.FromJson<AssemblyDefinitionSurrogate>(allText);

            return surrogate.name;
        }

        private static void CreateDebugStyle() {
            DebugStyle = new GUIStyle();

            var debugColor = Color.magenta;
            debugColor.a = DebugStyleBackgroundAlpha;

            DebugStyle.normal.background = CreateTexture(2, 2, debugColor);
        }

        /// <summary>
        ///     Converts the entire rect of a <see cref="UnityEditorInternal.ReorderableList" /> element into a rect used for displaying a field
        /// </summary>
        public static Rect GetReorderableListElementFieldRect(Rect elementRect) {
            elementRect.height = EditorGUIUtility.singleLineHeight;
            elementRect.y++;

            return elementRect;
        }

        public static bool SupportsMultiLine(Type type) {
            return type.GetCustomAttributes(typeof(MultiLine), true).Length > 0;
        }

        public static bool HasPropertyDrawer(Type type) {
            if (HasBuiltinPropertyDrawer(type))
                return true;

            if (_propertyDrawerGraph == null)
                CreatePropertyDrawerGraph();

            return _propertyDrawerGraph.HasPropertyDrawer(type);
        }

        private static bool HasBuiltinPropertyDrawer(Type type) {
            if (type.IsPrimitive || type == typeof(string) || IsFromUnityAssembly(type))
                return true;

            return false;
        }

        private static bool IsFromUnityAssembly(Type type) {
            return type.Assembly == typeof(GameObject).Assembly;
        }

        [DidReloadScripts]
        private static void OnProjectReloaded() {
            _propertyDrawerGraph = null;
        }

        private static Texture2D CreateTexture(int width, int height, Color col) {
            var pix = new Color[width * height];
            for (var i = 0; i < pix.Length; ++i) pix[i] = col;
            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private class AssemblyDefinitionSurrogate {
            public readonly string name = "";
        }

        /// <summary>
        ///     Goes through the entirety of the project and collects data about custom property drawers
        /// </summary>
        private class PropertyDrawerGraph {
            private readonly List<Assembly> _checkedAssemblies = new();
            private readonly List<Type> _supportedInheritedTypes = new();
            private readonly List<Type> _supportedTypes = new();

            public bool HasPropertyDrawer(Type type) {
                foreach (var supportedType in _supportedTypes)
                    if (supportedType == type)
                        return true;

                foreach (var inheritedSupportedType in _supportedInheritedTypes)
                    if (type.IsSubclassOf(inheritedSupportedType))
                        return true;

                return false;
            }

            public void CreateGraph(Assembly assembly) {
                if (_checkedAssemblies.Contains(assembly))
                    return;

                _checkedAssemblies.Add(assembly);

                foreach (var type in assembly.GetTypes()) {
                    var attributes = type.GetCustomAttributes(typeof(CustomPropertyDrawer), false);

                    foreach (var attribute in attributes)
                        if (attribute is CustomPropertyDrawer) {
                            var drawerData = attribute as CustomPropertyDrawer;

                            var useForChildren = (bool)typeof(CustomPropertyDrawer).GetField("m_UseForChildren", _fieldBindingsFlag).GetValue(drawerData);
                            var targetType = (Type)typeof(CustomPropertyDrawer).GetField("m_Type", _fieldBindingsFlag).GetValue(drawerData);

                            if (useForChildren)
                                _supportedInheritedTypes.Add(targetType);
                            else
                                _supportedTypes.Add(targetType);
                        }
                }
            }
        }
    }
}