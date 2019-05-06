using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EditorGUIUtils {
    public static float GetDisplayNameFieldWidth(float fieldWidth) {
        float minPropertyWidth = 250f;
        float minDisplayNameWidth = 150f;
        float displayNameScale = .42f;

        return fieldWidth < minPropertyWidth ? minDisplayNameWidth : fieldWidth * displayNameScale;
    }

    public static bool RefreshButton() {
        return GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Refresh"), GUILayout.Width(30));
    }
}
