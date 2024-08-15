using UnityEngine;

namespace EditorScripts.HideIf {
    public class HideIfAttribute : PropertyAttribute {
        public string ConditionalSourceField;

        public HideIfAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
        }
    }
}