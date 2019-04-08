using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Saves : MonoBehaviour {

    [System.Serializable]
    public class PlatformMethod {
        public RuntimePlatform platform;
        public SaveMethod method;
    }

    [SerializeField] private PlatformMethod[] _platformMethods;

	[SerializeField] private SaveMethod _defaultMethod;

    private SaveMethod _currentMethod;

    [SerializeField] private bool _saveOnPause;
    [SerializeField] private bool _saveOnQuit;

    private static Saves _instance;

    private string _saveKey;

    public static Saves instance {
        get {
            return _instance;
        }
    }

    private Hash _saveData;

    void Awake() {
        _instance = this;
        SetMethod();
    }

    private void SetMethod() {
        foreach (PlatformMethod pm in _platformMethods) {
            if (pm.platform == Application.platform) {
                _currentMethod = pm.method;
                return;
            }
        }
        _currentMethod = _defaultMethod;
    }

    public void ProcessFileList(Action<IEnumerable<string>, Exception> callback) {
        _currentMethod.LoadSavesList(callback);
    }

    public void LoadData(string key, Action<bool> callback = null) {
        Debug.Log(_currentMethod);
        _currentMethod.Load(key, (h, e) => {
            if (e == null && callback != null) {
                if (h == null) {
                    callback(false);
                    _saveKey = key;
                    _saveData = new Hash();
                } else {
                    _saveKey = key;
                    _saveData = h;
                    callback(true);
                }
            } else if (e != null) {
                Debug.LogError(e.Message);
                Debug.LogError(e.StackTrace);
                if (callback != null) {
                    callback(false);
                }
            }
        });
    }

    public object LoadObject(string key, object defaultValue = null) {
        if (_saveData == null) {
            throw new UnityException("Save data not loaded");
        }
        if (_saveData.ContainsKey(key)) {
            return _saveData[key];
        }
        return defaultValue;
    }

    public Hash LoadHash(string key) {
        return Hash.FromObject(LoadObject(key, null));
    }

    public string LoadString(string key, string defaultValue = null) {
        object value = LoadObject(key, defaultValue);
        return value == null ? null : value.ToString();
    }

    public bool LoadBool(string key, bool defaultValue = false) {
        object value = LoadObject(key, defaultValue);
        return (bool)value;
    }

    public int LoadInt(string key, int defaultValue = 0) {
        object value = LoadObject(key, defaultValue);
        return (int)value;
    }

    public double LoadDouble(string key, double defaultValue = 0) {
        object value = LoadObject(key, defaultValue);
        return (double)value;
    }

    public float LoadFloat(string key, float defaultValue = 0) {
        object value = LoadObject(key, defaultValue);
        return (float)value;
    }

    public long LoadLong(string key, long defaultValue = 0) {
        object value = LoadObject(key, defaultValue);
        return (long)value;
    }

    public T LoadISavable<T>(string key, T savable) where T: ISaveble {
        string json = LoadString(key);
        var hash = Hash.FromJSON(json);
        savable.FromJSON(hash);
        return savable;
    }

    public void Save(string key, object value) {
        var newValue = (value is ISaveble) ? ((ISaveble)value).ToJSON() : value;
        _saveData.AddOrSet(key, newValue);
    }

    public void EraseSave(string key = null) {
        if (string.IsNullOrEmpty(key)) {
            _saveData = new Hash();
        } else {
            _saveData.Remove(key);
        }
    }

    public bool ContainsSave(string key) {
        if (string.IsNullOrEmpty(key)) {
            return _saveData != null && _saveData.Count > 0;
        }
        return _saveData.ContainsKey(key);
    }

    void OnApplicationQuit() {
        if (_saveOnQuit) {
            if (_saveData != null) {
                _currentMethod.Save(_saveKey, _saveData.ToJSONString(), (b, e) => { });
            } else {
                Debug.Log("Nothing to save");
            }
        }
    }
}
