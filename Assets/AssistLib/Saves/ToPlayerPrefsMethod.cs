using System.Collections.Generic;
using UnityEngine;

public class ToPlayerPrefsMethod : SaveMethod {

    private string _savesKey = "SaveGames";

    public override void Load(string key, System.Action<Dictionary<string, object>, System.Exception> complete) {
        if (complete != null) {
            if (_allSaves.ContainsKey(key)) {
                complete(_allSaves.GetChild(key), null);
            } else { 
                complete(null, null);
            }
        }
    }

    public override void Save(string key, string toSave, System.Action<bool, System.Exception> complete) {
        var dict = _allSaves;
        dict.AddOrSet(key, toSave);
        _allSaves = dict;
    }

    public override void LoadSavesList(System.Action<System.Collections.Generic.IEnumerable<string>, System.Exception> complete) {
        if (complete != null) {
            complete(_allSaves.Keys, null);
        }
    }

    public override void Create(string key, System.Action<bool, System.Exception> complete) {
        _allSaves.AddOrSet(key, null);
    }

    private Dictionary<string, object> _allSaves {
        get {
            var allSaves = PlayerPrefs.GetString(_savesKey);
            var savesHash = JSONUtuls.Deserialize(allSaves);
            return savesHash == null ? new Dictionary<string, object>() : savesHash;
        }
        set {
            PlayerPrefs.SetString(_savesKey, JSONUtuls.Serialize(value));
            PlayerPrefs.Save();
        }
    }
}