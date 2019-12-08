using System.Linq;
using UnityEditor;

namespace c1tr00z.AssistLib.DataBase.Editor {

    public class DBItemsPostprocessor : AssetPostprocessor {

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths) {
            var reCollectDB = false;

            reCollectDB =
                ContainsAsset(importedAssets) || ContainsAsset(deletedAssets) || ContainsAsset(movedAssets);

            if (reCollectDB) {
                ItemsEditor.AutoCollect();
            }
        }

        private static bool ContainsAsset(string[] paths) {
            return paths.Any(p => p.ToLower().EndsWith(".asset") || p.Contains("@"));
        }
    }
}
