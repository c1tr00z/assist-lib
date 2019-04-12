using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.EditorTools {
    public class EditorTool {

        public void Draw() {
            DrawTitle();
            DrawInterface();
        }

        public virtual void Init(Dictionary<string, object> settings) {
            
        }

        public virtual void Save(Dictionary<string, object> settings) {

        }

        protected virtual void DrawInterface() {
            EditorGUILayout.LabelField("Nothing here...");
        }

        private void DrawTitle() {
            var editorToolName = (EditorToolName)System.Attribute.GetCustomAttribute(GetType(), typeof(EditorToolName));
            var toolLabel = editorToolName != null ? editorToolName.toolName : GetType().ToString();
            GUILayout.Label(toolLabel, EditorStyles.boldLabel);
        }

        protected bool Button(string caption, params GUILayoutOption[] options) {
            return GUILayout.Button(caption, options);
        }

        protected void Label(string caption) {
            EditorGUILayout.LabelField(caption);
        }
    }
}
