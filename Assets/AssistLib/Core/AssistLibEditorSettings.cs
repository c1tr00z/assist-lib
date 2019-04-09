using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AssistLibEditorSettings {

    private static Dictionary<string, object> _editorSettingsData;

    public static string editorSettingsKey {
        get {
            return "AssistLibEditorSettings";
        }
    }

    private static void CheckLoading() {
        if (_editorSettingsData == null) {
            var settingsJson = EditorPrefs.GetString(editorSettingsKey);
            if (string.IsNullOrEmpty(settingsJson)) {
                _editorSettingsData = new Dictionary<string, object>();
            } else {
                _editorSettingsData = JSONUtuls.Deserialize(settingsJson);
            }
        }
    }

    public static Dictionary<string, object> GetDataNode(string key) {
        CheckLoading();
        return _editorSettingsData.ContainsKey(key) ? (Dictionary<string, object>)_editorSettingsData[key] : new Dictionary<string, object>();
    }

    public static void SetDataNode(string key, Dictionary<string, object> node) {
        _editorSettingsData.AddOrSet(key, node);
        Save();
    }

    public static void Save() {
        EditorPrefs.SetString(editorSettingsKey, JSONUtuls.Serialize(_editorSettingsData));
    }
}
