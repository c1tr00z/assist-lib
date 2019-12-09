using c1tr00z.AssistLib.DataBase.Editor;
using UnityEditor;

namespace c1tr00z.AssistLib.Modules.Editor {
    public static class EditorModulesItemsUtils {
        public static class EditorUIItemsUtils {
            [MenuItem("Assets/AssistLib/Create Module DBEntry")]
            public static void CreateModuleDBEntry() {
                ItemsEditor.CreateItem<ModuleDBEntry>();
            }
        }
    }
}