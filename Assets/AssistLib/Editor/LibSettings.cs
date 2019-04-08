using UnityEngine;
using UnityEditor;
using System.Collections;

public class LibSettingsWindow : EditorWindow {

    private string localizationDocKey;
    private int localizationDocFileExtension;

    public LibSettingsWindow() {
        localizationDocKey = AssistLibSettings.localizationDocumentKey;
    }

    [MenuItem("Assist/Settings")]
    public static void ShowSettingsWindow() {
        EditorWindow.GetWindow(typeof(LibSettingsWindow), true);
    }

    void OnGUI() {
        GUILayout.Label("Localization Settings", EditorStyles.boldLabel);
        localizationDocKey = EditorGUILayout.TextField("Localization Document Key", localizationDocKey);
        if (GUILayout.Button("Save")) {
            SaveSettings();
        }
    }

    private void SaveSettings() {
        if (!string.IsNullOrEmpty(localizationDocKey)) {
            AssistLibSettings.localizationDocumentKey = localizationDocKey;
        }
        PlayerPrefs.Save();
    }
}
