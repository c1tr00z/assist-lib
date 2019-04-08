using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using System;

public class Hash : Dictionary<string, object> {

    public bool TryGet<T>(string key, out T value) {
        if (ContainsKey(key)) {
            var obj = this[key];
            if (obj is T) {
                value = (T)obj;
                return true;
            }
        }

        value = default(T);
        return false;
    }

    public T Get<T>(string key) {

        if (ContainsKey(key)) {
            var obj = this[key];
//			Debug.LogError(string.Format("key: {0} | type: {1} | obj: {2}", key, typeof(T), obj));
            if (typeof(T) == typeof(Hash)) {
                var dict = Get<Dictionary<string, object>>(key);
                if (dict == null) {
                    return default(T);
                }
                var hash = new Hash();
                dict.ForEach(kvp => hash.AddOrSet(kvp.Key, kvp.Value));
                return (T)(object)hash;
            } else if (typeof(T) == typeof(IEnumerable<Hash>)) {
                var list = (IEnumerable<Dictionary<string, object>>)obj;
                var hashList = new List<Hash>();
                if (list == null) {
                    return default(T);
                }
                list.ForEach(dic => hashList.Add(Hash.FromDictionary(dic)));
                return (T)(object)list.ToArray();
            } else if (obj is T) {
                var value = (T)obj;
                return value;
            }
        }

        return default(T);
    }

    public Hash GetChild(string key) {
        return Get<Hash>(key);
    }

    public int GetInt(string key) {
		object val = null;
		if (ContainsKey (key)) {
			val = this [key];
			if (val.GetType() == typeof(Int16)) {
				return (int)Get<Int16>(key);
			} else if (val.GetType() == typeof(Int32)) {
				return (int)Get<Int32>(key);
			} else if (val.GetType() == typeof(Int64)) {
				return (int)Get<Int64>(key);
			}
		}
		return 0;
    }

    public static Hash FromJSON(string json) {
        if (string.IsNullOrEmpty(json)) {
            return null;
        }
        var dic = Json.Deserialize(json) as Dictionary<string, object>;
        if (dic == null || dic.Count == 0   ) {
            return null;
        }
        var hash = new Hash();
        dic.ForEach(kvp => hash.AddOrSet(kvp.Key, kvp.Value));
        return hash;
    }

    public string ToJSONString() {
        return Json.Serialize(this);
    }

    public override string ToString() {
        return ToJSONString();
    }

    public static Hash FromObject(object obj) {
        if (obj is Hash) {
            return (Hash)obj;
        } else if (obj is Dictionary<string, object>) {
            return FromDictionary((Dictionary<string, object>)obj);
        }
        return null;
    }

    public static Hash FromDictionary(Dictionary<string, object> dic) {
        var hash = new Hash();
        dic.ForEach(kvp => hash.AddOrSet(kvp.Key, kvp.Value));
        return hash;
    }

	public static Hash FromDictionary(Dictionary<string, Hash> dic) {
		var hash = new Hash();
		dic.ForEach(kvp => hash.AddOrSet(kvp.Key, kvp.Value));
		return hash;
	}
}
