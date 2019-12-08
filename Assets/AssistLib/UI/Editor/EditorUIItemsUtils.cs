using c1tr00z.AssistLib.DataBase.Editor;
using UnityEditor;

namespace c1tr00z.AssistLib.UI.Editor {
    public static class EditorUIItemsUtils {
        [MenuItem("Assets/AssistLib/Create UI Frame DBEntry")]
        public static void CreateUIFrameDBEntry() {
            ItemsEditor.CreateItem<UIFrameDBEntry>();
        }
    }
}
