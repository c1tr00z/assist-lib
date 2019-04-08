using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

public class EditorTools : EditorWindow {

    private List<EditorTool> tools = null;

	[MenuItem("Assist/Tools")]
    public static void ShowSettingsWindow() {
        EditorWindow.GetWindow(typeof(EditorTools), true);
    }

    void OnGUI() {
        GUILayout.Label("Editor tools", EditorStyles.boldLabel);

        if (GUILayout.Button("1")) {
            tools = new List<EditorTool>();
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(EditorTool)))) {
                var toolName = (EditorToolName)System.Attribute.GetCustomAttribute(t, typeof(EditorToolName));
                if (toolName != null && !string.IsNullOrEmpty(toolName.toolName)) {
                    var tool = (EditorTool) Activator.CreateInstance(t);
                    tools.Add(tool);
                }
            }
        }

        if (tools != null) {
            tools.ForEach(tool => tool.Draw());
        }
    }
}
