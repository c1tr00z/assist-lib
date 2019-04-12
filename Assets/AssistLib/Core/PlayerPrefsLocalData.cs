using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsLocalData : MonoBehaviour {
    private static Dictionary<string, object> _playerPrefsLocalData;

    public static string editorSettingsKey {
        get {
            return "AssistLib";
        }
    }

    private static void CheckLoading() {
        if (_playerPrefsLocalData == null) {
            var settingsJson = PlayerPrefs.GetString(editorSettingsKey);
            if (string.IsNullOrEmpty(settingsJson)) {
                _playerPrefsLocalData = new Dictionary<string, object>();
            } else {
                _playerPrefsLocalData = JSONUtuls.Deserialize(settingsJson);
            }
        }
    }

    public static Dictionary<string, object> GetDataNode(string key) {
        CheckLoading();
        return _playerPrefsLocalData.ContainsKey(key) ? (Dictionary<string, object>)_playerPrefsLocalData[key] : new Dictionary<string, object>();
    }

    public static void SetDataNode(string key, Dictionary<string, object> node) {
        _playerPrefsLocalData.AddOrSet(key, node);
        Save();
    }

    public static void Save() {
        PlayerPrefs.SetString(editorSettingsKey, JSONUtuls.Serialize(_playerPrefsLocalData));
    }
}
