using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ItemsEditor {

    [MenuItem("Assets/Create item")]
    public static void CreateItem() {

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

                var itemResource = itemPrefab.GetComponent<DBEntryResource>();
                if (itemResource == null) {
                    itemResource = itemPrefab.AddComponent(typeof(DBEntryResource)) as DBEntryResource;
                }

                itemResource.SetParent(i, "Prefab");
                EditorUtility.SetDirty(itemPrefab);
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
