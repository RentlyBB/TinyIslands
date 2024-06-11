using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class HierarchyWindowHeader {

    static HierarchyWindowHeader() {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    static void HierarchyWindowItemOnGUI(int instatnceID, Rect selectionRect) {
        var gameObject = EditorUtility.InstanceIDToObject(instatnceID) as GameObject;

        if (gameObject != null && gameObject.name.StartsWith("//", System.StringComparison.Ordinal)) {
            EditorGUI.DrawRect(selectionRect, Color.black);
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("/","").ToUpperInvariant());
        }
    }
}