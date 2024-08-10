using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ScriptableObjectArchitecture.Editor {
    public static class SO_CodeGenerator {
        public const int TYPE_COUNT = 6;

        private static readonly string[] _templateNames = new string[TYPE_COUNT] {
            "GameEventListenerTemplate",
            "GameEventTemplate",
            "ReferenceTemplate",
            "CollectionTemplate",
            "UnityEventTemplate",
            "VariableTemplate"
        };

        private static readonly string[] _targetFileNames = new string[TYPE_COUNT] {
            "{0}GameEventListener.cs",
            "{0}GameEvent.cs",
            "{0}Reference.cs",
            "{0}Collection.cs",
            "{0}UnityEvent.cs",
            "{0}Variable.cs"
        };

        private static string[] _targetDirectories;
        private static readonly string[] _templatePaths = new string[TYPE_COUNT];
        private static string[,] _replacementStrings;

        static SO_CodeGenerator() {
            CreateTargetDirectories();
            GatherFilePaths();
        }

        private static string TypeName => _replacementStrings[1, 1];

        private static void CreateTargetDirectories() {
            _targetDirectories = new string[TYPE_COUNT] {
                Application.dataPath + "/" + SOArchitecture_Settings.Instance.CodeGenerationTargetDirectory + "/Events/Listeners",
                Application.dataPath + "/" + SOArchitecture_Settings.Instance.CodeGenerationTargetDirectory + "/Events/Game Events",
                Application.dataPath + "/" + SOArchitecture_Settings.Instance.CodeGenerationTargetDirectory + "/References",
                Application.dataPath + "/" + SOArchitecture_Settings.Instance.CodeGenerationTargetDirectory + "/Collections",
                Application.dataPath + "/" + SOArchitecture_Settings.Instance.CodeGenerationTargetDirectory + "/Events/Responses",
                Application.dataPath + "/" + SOArchitecture_Settings.Instance.CodeGenerationTargetDirectory + "/Variables"
            };
        }

        private static void GatherFilePaths() {
            var assetPath = Application.dataPath;
            var folderToStartSearch = Directory.GetParent(assetPath).FullName;

            var foldersToCheck = new Queue<string>();
            foldersToCheck.Enqueue(folderToStartSearch);

            while (foldersToCheck.Count > 0) {
                var currentDirectory = foldersToCheck.Dequeue();

                foreach (var filePath in Directory.GetFiles(currentDirectory)) {
                    var fileName = Path.GetFileName(filePath);

                    for (var i = 0; i < TYPE_COUNT; i++)
                        if (_templateNames[i] == fileName)
                            _templatePaths[i] = filePath;
                }

                foreach (var subDirectory in Directory.GetDirectories(currentDirectory)) foldersToCheck.Enqueue(subDirectory);
            }

            //Double check that all filepaths were found
            for (var i = 0; i < TYPE_COUNT; i++)
                if (_templatePaths[i] == default)
                    Debug.LogError("Couldn't find path for " + _templatePaths[i]);
        }

        public static void Generate(Data data) {
            _replacementStrings = new string[4, 2] {
                { "$TYPE$", data.TypeName },
                { "$TYPE_NAME$", CapitalizeFirstLetter(data.TypeName) },
                { "$MENU_NAME$", data.MenuName },
                { "$ORDER$", data.Order.ToString() }
            };

            for (var i = 0; i < TYPE_COUNT; i++)
                if (data.Types[i])
                    GenerateScript(i);
        }

        private static void GenerateScript(int index) {
            var targetFilePath = GetTargetFilePath(index);
            var contents = GetScriptContents(index);

            if (File.Exists(targetFilePath) && !SOArchitecture_Settings.Instance.CodeGenerationAllowOverwrite) {
                Debug.Log("Cannot create file at " + targetFilePath + " because a file already exists, and overwrites are disabled");
                return;
            }

            Debug.Log("Creating " + targetFilePath);

            Directory.CreateDirectory(Path.GetDirectoryName(targetFilePath));
            File.WriteAllText(targetFilePath, contents);
        }

        private static string GetScriptContents(int index) {
            var templatePath = _templatePaths[index];
            var templateContent = File.ReadAllText(templatePath);

            var output = templateContent;

            for (var i = 0; i < _replacementStrings.GetLength(0); i++) output = output.Replace(_replacementStrings[i, 0], _replacementStrings[i, 1]);

            return output;
        }

        private static string GetTargetFilePath(int index) {
            return _targetDirectories[index] + "/" + string.Format(_targetFileNames[index], TypeName);
        }

        private static string CapitalizeFirstLetter(string input) {
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public struct Data {
            public bool[] Types;
            public string TypeName;
            public string MenuName;
            public int Order;
        }
    }
}