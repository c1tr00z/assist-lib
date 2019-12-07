using System.Linq;
using UnityEditor;

public class DBItemsPostprocessor : AssetPostprocessor {
    
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths) {
        var reCollectDB = false;

        reCollectDB =
            ContainsAsset(importedAssets) || ContainsAsset(deletedAssets) || ContainsAsset(movedAssets);

        if (reCollectDB) {
            ItemsEditor.CollectItems();
        }
    }

    private static bool ContainsAsset(string[] paths) {
        return paths.Any(p => p.ToLower().EndsWith(".asset") || p.Contains("@"));
    }
}