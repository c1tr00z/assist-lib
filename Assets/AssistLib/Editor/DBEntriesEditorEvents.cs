using UnityEditor;

namespace AssistLib.Editor {
    [InitializeOnLoad]
    public class DBEntriesEditorEvents {
        public DBEntriesEditorEvents() {
            EditorApplication.projectChanged += EditorApplicationOnProjectChanged;
        }

        private void EditorApplicationOnProjectChanged() {
            ItemsEditor.CollectItems();
        }
    }
}