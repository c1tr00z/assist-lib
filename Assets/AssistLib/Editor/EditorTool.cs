using UnityEngine;
using UnityEditor;
using System.Collections;

public class EditorTool {

    public void Draw() {
        DrawInterface();
    }

    protected virtual void DrawInterface() {
        EditorGUILayout.LabelField("Nothing here...");
    }

    protected bool Button(string caption) {
        return GUILayout.Button(caption);
    }

    protected void Label(string caption){
        EditorGUILayout.LabelField(caption);
    }
}
