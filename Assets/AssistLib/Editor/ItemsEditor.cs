﻿using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ItemsEditor {

    [MenuItem("Assets/Create DBEntry")]
    public static void CreateDBEntry() {

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "") {
            path = "Assets";
        } else if (Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        DBEntry item = AssetDBUtils.CreateScriptableObject<DBEntry>(path, "New DBEntry");

        CollectItems();

        Selection.activeObject = item;
    }

    public static void CreateItem<T>() where T: ScriptableObject {
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (path == "") {
            path = "Assets";
        } else if (Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        var item = AssetDBUtils.CreateScriptableObject<T>(path, string.Format("New {0}", typeof(T).Name));

        CollectItems();

        Selection.activeObject = item;
    }

    [MenuItem("Assist/Create DB")]
    public static void CreateDB() {
        if (DB.Get<DB>() != null) {
            return;
        }

        PathUtils.CreatePath("Resources");  
        AssetDBUtils.CreateScriptableObject<DB>(PathUtils.Combine("Assets", "Resources"), "DB");
        CollectItems();
    }

    [MenuItem("Assist/Create AppSettings")]
    public static void CreateAppSettings() {
        if (DB.Get<DB>() == null) {
            return;
        }

        if (DB.Get<AppSettings>() != null) {
            return;
        }

        PathUtils.CreatePath("AssistLib", "Resources");
        AssetDBUtils.CreateScriptableObject<AppSettings>(PathUtils.Combine("Assets", "AssistLib", "Resources"), "AppSettings");
        CollectItems();
    }

    [MenuItem("Assist/Collect items")]
    public static void CollectItems() {
        var itemsObject = Resources.Load<DB>("DB");
        var dirs = new List<string>();
        var items = Resources.LoadAll<DBEntry>("");
        itemsObject.paths = items.Select(i => {
            var path = AssetDatabase.GetAssetPath(i).Replace(".asset", "");
            path = (path.Contains("Resources")) ?
                path.Replace(path.Substring(0, path.IndexOf("Resources") + "Resources/".Length), "") :
                path;
            return path;
        }).ToArray();
        foreach (DBEntry i in items) {
            var itemPrefab = i.LoadPrefab<GameObject>();
            if (itemPrefab != null) {

#if UNITY_2018_3_OR_NEWER
                var path = AssetDatabase.GetAssetPath(itemPrefab);
                var prefabGO = PrefabUtility.LoadPrefabContents(path);
                var itemResource = prefabGO.GetComponent<DBEntryResource>();
                if (itemResource == null) {
                    itemResource = prefabGO.AddComponent(typeof(DBEntryResource)) as DBEntryResource;
                }
                itemResource.SetParent(i, "Prefab");
                PrefabUtility.SaveAsPrefabAsset(prefabGO, path);
                EditorUtility.SetDirty(itemPrefab);
#else
                var itemResource = itemPrefab.GetComponent<DBEntryResource>();
                if (itemResource == null) {
                    itemResource = itemPrefab.AddComponent(typeof(DBEntryResource)) as DBEntryResource;
                }

                itemResource.SetParent(i, "Prefab");
                EditorUtility.SetDirty(itemPrefab);
#endif
            }

        }
        EditorUtility.SetDirty(itemsObject);
    }

    public static void GetDirectories(string startPath, string path, List<string> directories) {
        var info = new DirectoryInfo(path);
        var dirs = info.GetDirectories();
        foreach (DirectoryInfo d in dirs) {
            var dir = d.ToString();
            var newDir = dir.Replace(path, "");
            newDir = newDir.StartsWith("Resources") ? newDir.Substring("Resources".Length) : newDir;
            newDir = newDir.StartsWith("\\") ? newDir.Substring("\\".Length) : newDir;
            if (!string.IsNullOrEmpty(newDir)) {
                directories.Add(newDir);
            }
            GetDirectories(startPath, dir, directories);
        }
    }
}
