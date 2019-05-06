using UnityEngine;

namespace c1tr00z.AssistLib.Modules.Editor {
    public static class ModulesEditorUtils {

        public static void CreateModule<T>(string moduleName, System.Action<T> onCreate) where T : Component {
            PathUtils.CreatePath("AssistLib", "Resources", "Modules");
            var moduleDBEntry = AssetDBUtils.CreateScriptableObject<ModuleDBEntry>(PathUtils.Combine("Assets", "AssistLib", "Resources", "Modules"), moduleName);
            AssetDBUtils.CreatePrefab(moduleDBEntry, onCreate);
            ItemsEditor.CollectItems();
        }
    }
}