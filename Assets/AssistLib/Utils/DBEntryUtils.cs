using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBEntryUtils {
    
    public static string GetPath(this DBEntry item) {
        return DB.GetPath(item);
    }

    public static T Load<T>(this DBEntry item, string key) where T : Object {
        return (T)Resources.Load(GetPath(item) + "@" + key, typeof(T));
    }

    public static T LoadPrefab<T>(this DBEntry item) where T : Object {
        return (T)Resources.Load(GetPath(item) + "@Prefab", typeof(T));
    }

    public static string LoadText(this DBEntry item) {
        var textAsset = (TextAsset)Resources.Load(GetPath(item) + "@Text", typeof(TextAsset));
        return textAsset.text;
    }

    public static SpriteRenderer LoadSpriteRenderer(this DBEntry item, string key) {
        return Load<SpriteRenderer>(item, key);
    }

    public static Sprite LoadSprite(this DBEntry item, string key) {
        return item.Load<Sprite>(key);
    }

    public static Sprite LoadIcon(this DBEntry item) {
        return item.LoadSprite("Icon");
    }
}
