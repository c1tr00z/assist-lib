using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.UI.Editor {
    public static class EditorUIItemsUtils {
        [MenuItem("Assets/AssistLib/Create UI Frame DBEntry")]
        public static void CreateUIFrameDBEntry() {
            ItemsEditor.CreateItem<UIFrameDBEntry>();
        }
    }
}
