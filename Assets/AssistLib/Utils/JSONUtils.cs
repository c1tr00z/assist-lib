using MiniJSON;
using System.Collections.Generic;

public static class JSONUtuls {

    public static string Serialize(object objectToSerialize) {
        return Json.Serialize(objectToSerialize);
    }

    public static Dictionary<string, object> Deserialize(string jsonString) {
        return (Dictionary<string, object>)Json.Deserialize(jsonString);
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
}
