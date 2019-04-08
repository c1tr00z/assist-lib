using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToPlayerPrefsMethod : SaveMethod {

    private string _savesKey = "SaveGames";

    public override void Load(string key, System.Action<Hash, System.Exception> complete) {
        if (complete != null) {
            if (_allSaves.ContainsKey(key)) {
                complete(_allSaves.Get<Hash>(key), null);
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

    private Hash _allSaves {
        get {
            var allSaves = PlayerPrefs.GetString(_savesKey);
            var savesHash = Hash.FromJSON(allSaves);
            return savesHash == null ? new Hash() : savesHash;
        }
        set {
            PlayerPrefs.SetString(_savesKey, value.ToJSONString());
            PlayerPrefs.Save();
        }
    }
}