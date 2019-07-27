using MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class JSONUtuls {

    public static string Serialize(object objectToSerialize) {
        return Json.Serialize(objectToSerialize);
    }

    public static Dictionary<string, object> Deserialize(string jsonString) {
        var deserialized = Json.Deserialize(jsonString);

        if (deserialized == null) {
            Debug.LogError("JSON string is corrupted!");
            Debug.LogError("Corrupted json: " + jsonString);
            return new Dictionary<string, object>();
        }

        return (Dictionary<string, object>)deserialized;
    }

    public static T Get<T>(this Dictionary<string, object> json, string key) {
        return json.ContainsKey(key) ? (T)json[key] : default(T);
    }

    public static IEnumerable<T> GetIEnumerable<T>(this Dictionary<string, object> json, string key) {
        var value = json.ContainsKey(key) ? json.Get<object>(key) : null;
        if (value == null) {
            return new List<T>();
        }

        return value is T[]
            ? (T[])value
            : value is List<object>
                ? ((List<object>)value).SelectNotNull(o => (T)o)
                : new List<T>();
    }

    public static Dictionary<string, object> GetChild(this Dictionary<string, object> json, string key) {
        return json.ContainsKey(key) ? (Dictionary<string, object>)json[key] : new Dictionary<string, object>();
    }

    public static string GetString(this Dictionary<string, object> json, string key) {
        return json.ContainsKey(key) ? json[key].ToString() : null;
    }

    public static int GetInt(this Dictionary<string, object> json, string key) {
        var stringValue = json.GetString(key);
        int value = 0;
        if (!string.IsNullOrEmpty(stringValue) && int.TryParse(stringValue, out value)) {
            return value;
        }
        return 0;
    }

    public static float GetFloat(this Dictionary<string, object> json, string key) {
        var stringValue = json.GetString(key);
        float value = 0;
        if (!string.IsNullOrEmpty(stringValue) && float.TryParse(stringValue, out value)) {
            return value;
        }
        return 0f;
    }

    public static bool GetBool(this Dictionary<string, object> json, string key, bool defaultValue = false) {
        var stringValue = json.GetString(key);
        bool value = false;
        if (!string.IsNullOrEmpty(stringValue) && bool.TryParse(stringValue, out value)) {
            return value;
        }
        return defaultValue;
    }

    public static Vector2 GetVector2(this Dictionary<string, object> json, string key) {
        var stringValue = json.GetString(key);
        Vector2 value;
        if (VectorUtils.TryParse(stringValue, out value)) {
            return value;
        }
        return Vector2.zero;
    }

    public static Vector3 GetVector3(this Dictionary<string, object> json, string key) {
        var stringValue = json.GetString(key);
        Vector3 value;
        if (VectorUtils.TryParse(stringValue, out value)) {
            return value;
        }
        return Vector3.zero;
    }
}
