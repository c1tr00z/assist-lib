using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DB : DBEntry {
    public string[] paths;

    private Dictionary<DBEntry, string> _items { get; set; }

    private static DB _instance;

    private static void CheckItems() {
        if (_instance == null) {
            _instance = Resources.Load<DB>("DB");
        }
        if (_instance == null) {
            return;
        }
        if (_instance._items == null) {
            _instance._items = new Dictionary<DBEntry, string>();
        } else {
            //_instance._items.Clear();
        }
        if (_instance._items.Count == 0) {
            _instance.paths.SelectNotNull().ForEach(path => {
                var dbItem = Resources.Load<DBEntry>(path);
                if (dbItem != null) {
                    _instance._items.Add(dbItem, path);
                }
            });
        }
    }

    public static T Get<T>(string name) where T: DBEntry {
        CheckItems();
        return GetAll<T>().SelectNotNull().Where(item => item != null && item.name == name).First();
    }

    public static T Get<T>() where T : DBEntry {
        CheckItems();
        return GetAll<T>().SelectNotNull().First();
    }

    public static IEnumerable<T> GetAll<T>() where T : DBEntry {
        CheckItems();
        var items = new List<T>();
        if (_instance == null) {
            Debug.LogError("No DB instance");
            return items;
        }
        return _instance._items.Keys.SelectNotNull(i => i as T);
    }

    public static string GetPath(DBEntry item) {
        CheckItems();
        return _instance._items.ContainsKey(item) ? _instance._items[item] : null;
    }
}
