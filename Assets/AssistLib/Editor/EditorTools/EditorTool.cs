using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.EditorTools {
    public class EditorTool {

        private bool _toggle = false;

        private string _toolLabel = "Empty...";

        public void Draw() {
            if (DrawTitle()) {
                GUILayout.Label(_toolLabel, EditorStyles.boldLabel);
                DrawInterface();
            }
        }

        public virtual void Init(Dictionary<string, object> settings) {
            
        }

        public virtual void Save(Dictionary<string, object> settings) {

        }

        protected virtual void DrawInterface() {
            EditorGUILayout.LabelField("Nothing here...");
        }

        private bool DrawTitle() {
            var editorToolName = (EditorToolName)System.Attribute.GetCustomAttribute(GetType(), typeof(EditorToolName));
            _toolLabel = editorToolName != null ? editorToolName.toolName : GetType().ToString();
            _toggle = EditorGUILayout.Foldout(_toggle, _toolLabel);
            return _toggle;
        }

        protected bool Button(string caption, params GUILayoutOption[] options) {
            return GUILayout.Button(caption, options);
        }

        protected void Label(string caption) {
            EditorGUILayout.LabelField(caption);
        }
    }
}
