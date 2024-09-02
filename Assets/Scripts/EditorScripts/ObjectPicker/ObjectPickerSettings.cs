using UnityEngine;

namespace EditorScripts {
    [CreateAssetMenu(fileName = "ObjectPickerSettings", menuName = "Custom Tools/Object Picker Settings")]
    public class ObjectPickerSettings : ScriptableObject
    {
        public string[] ignoreTags;
        public LayerMask ignoreLayers;
    }
}