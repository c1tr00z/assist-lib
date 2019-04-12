using UnityEditor;
using UnityEngine;

public static class AssetDBUtils {

    public static T CreateScriptableObject<T>(string path) where T : ScriptableObject {
        return CreateScriptableObject<T>(path, typeof(T).ToString(), false);
    }

    public static T CreateScriptableObject<T>(string path, string name) where T : ScriptableObject {
        return CreateScriptableObject<T>(path, name, false);
    }

    public static T CreateScriptableObject<T>(string path, string name, bool select) where T : ScriptableObject {
        T item = ScriptableObject.CreateInstance<T>();

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", path, name));

        AssetDatabase.CreateAsset(item, assetPathAndName);

        AssetDatabase.SaveAssets();

        if (select) {
            Selection.activeObject = item;
        }

        return item;
    }
}
