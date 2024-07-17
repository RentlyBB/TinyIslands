using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HierarchyWindowHeader {

    static HierarchyWindowHeader() {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    private static void HierarchyWindowItemOnGUI(int instatnceID, Rect selectionRect) {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instatnceID) as GameObject;

        if (gameObject != null && gameObject.name.StartsWith("//", StringComparison.Ordinal)) {
            EditorGUI.DrawRect(selectionRect, Color.black);
            EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("/", "").ToUpperInvariant());
        }
    }
}