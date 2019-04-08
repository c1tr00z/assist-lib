using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

[CustomEditor(typeof(ToTXTFile))]
public class ToFileEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var saver = (ToFile)target;

        if (GUILayout.Button("Open Folder")) {
            EditorUtility.RevealInFinder(Path.Combine(Application.persistentDataPath, saver.savesPath));
        }
    }
}
