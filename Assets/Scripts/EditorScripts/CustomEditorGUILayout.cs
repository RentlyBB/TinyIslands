using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EditorScripts {
    public static class CustomEditorGUILayout
    {
        public static LayerMask LayerMaskField(string label, LayerMask selected)
        {
            // Convert the LayerMask to an int
            int selectedValue = InternalEditorUtility.LayerMaskToConcatenatedLayersMask(selected);

            // Get the current layers in Unity
            string[] layers = InternalEditorUtility.layers;

            // Convert the int value to a LayerMask using the LayerMaskField method
            selectedValue = EditorGUILayout.MaskField(label, selectedValue, layers);

            // Convert the LayerMask back to an int
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(selectedValue);
        }
    }
}