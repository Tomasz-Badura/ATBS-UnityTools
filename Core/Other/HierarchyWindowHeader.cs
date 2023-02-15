﻿using UnityEngine;
using UnityEditor;
namespace ATBS.Editor
{
    [InitializeOnLoad]
    public static class HierarchySectionHeader
    {
        static HierarchySectionHeader()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        }

        static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (gameObject != null && gameObject.name.StartsWith("//", System.StringComparison.Ordinal))
            {
                EditorGUI.DrawRect(selectionRect, Color.black);
                EditorGUI.DropShadowLabel(selectionRect, gameObject.name.Replace("/", "").ToUpperInvariant());
            }
        }
    }
}