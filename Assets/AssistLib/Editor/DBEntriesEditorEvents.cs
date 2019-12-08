using UnityEditor;

namespace c1tr00z.AssistLib.DataBase.Editor {
    [InitializeOnLoad]
    public class DBEntriesEditorEvents {
        public DBEntriesEditorEvents() {
            EditorApplication.projectChanged += EditorApplicationOnProjectChanged;
            ItemsEditor.CollectItems();
        }

        private void EditorApplicationOnProjectChanged() {
            ItemsEditor.AutoCollect();
        }
    }
}