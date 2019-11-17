using MiniJSON;
using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.Json;
using UnityEngine;

public static class JSONUtuls {

    public static string Serialize(object objectToSerialize) {
        if (objectToSerialize is IJsonSerializable) {
            var json = new Dictionary<string, object>();
            (objectToSerialize as IJsonSerializable).Serialize(json);
            objectToSerialize = json;
        } else if (objectToSerialize is Dictionary<string, object>) {
            var newDic = new Dictionary<string, object>();
            (objectToSerialize as Dictionary<string, object>).ForEach(kvp => {
                var value = kvp.Value;
                if (value is IJsonSerializable) {
                    var json = new Dictionary<string, object>();
                    (value as IJsonSerializable).Serialize(json);
                    newDic.Add(kvp.Key, json);
                }
                else {
                    newDic.Add(kvp.Key, kvp.Value);
                }

                objectToSerialize = newDic;
            });
        } else if (objectToSerialize is Dictionary<object, object>) {
            //
        } else if (objectToSerialize is IEnumerable<object>) {
            var list = new List<object>();
            (objectToSerialize as IEnumerable<object>).ForEach(o => {
                if (o is IJsonSerializable) {
                    var json = new Dictionary<string, object>();
                    (o as IJsonSerializable).Serialize(json);
                    list.Add(json);
                }
                else {
                    list.Add(o);
                }

                objectToSerialize = list;
            });
        }
        
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
        return json.ContainsKey(key) 
            ? typeof(IJsonDeserializable).IsAssignableFrom(typeof(T)) 
                ? TryGetDeserializeable<T>(json)
                : (T)json[key] 
            : default(T);
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
    
    public static T TryGetDeserializeable<T>(this Dictionary<string, object> json) {
        if (typeof(T).IsSubclassOf(typeof(IJsonDeserializable))) {
            var deserializeable = (T) Activator.CreateInstance(typeof(T));
            ((IJsonDeserializable)deserializeable).Deserialize(json);
            return deserializeable;
        }

        return default(T);
    }

    public static T GetDeserializeable<T>(this Dictionary<string, object> json) where T : IJsonDeserializable {
        var deserializeable = (T) Activator.CreateInstance(typeof(T));
        deserializeable.Deserialize(json);
        return deserializeable;
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

    public static long GetLong(this Dictionary<string, object> json, string key) {
        var stringValue = json.GetString(key);
        long value = 0;
        if (!string.IsNullOrEmpty(stringValue) && long.TryParse(stringValue, out value)) {
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
