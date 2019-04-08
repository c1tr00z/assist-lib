using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ItemsEditor {

    [MenuItem("Assets/Create item")]
    public static void CreateItem() {
        DBEntry item = ScriptableObject.CreateInstance<DBEntry>();

        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (path == "") {
            path = "Assets";
        } else if (Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(DBEntry).ToString() + ".asset");

        AssetDatabase.CreateAsset(item, assetPathAndName);

        AssetDatabase.SaveAssets();

        CollectItems();

        Selection.activeObject = item;
    }
     
    public static T CreateOrGetItem<T>(string path, string name) where T : DBEntry {

        Debug.Log(name);

        if (path == "") {
            path = "Assets";
        } else if (Path.GetExtension(path) != "") {
            path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        var pathDir = new DirectoryInfo(Path.Combine(Application.dataPath, path.Replace("Assets\\", "").Replace("Assets/", "")));
        Debug.Log(pathDir.ToString());
        if (!pathDir.Exists) {
            pathDir.Create();
        }

        var item = DB.Get<T>(name);
        if (item == null) {
            item = ScriptableObject.CreateInstance<T>();
            
            string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, name) + ".asset");

            AssetDatabase.CreateAsset(item, assetPathAndName);

            AssetDatabase.SaveAssets();
        }

        return item;
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
