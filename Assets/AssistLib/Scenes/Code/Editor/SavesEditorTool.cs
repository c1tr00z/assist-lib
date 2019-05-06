using c1tr00z.AssistLib.EditorTools;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.Saves.Editor {
    [EditorToolName("Saves Tool")]
    public class SavesEditorTool : EditorTool {

        private static readonly string _moduleName = "Saves";

        private ModuleDBEntry _savesModule;

        private bool _checked = false;

        public override void Init(Dictionary<string, object> settings) {
            base.Init(settings);
            _checked = false;
        }

        protected override void DrawInterface() {

            EditorGUILayout.BeginHorizontal();
            if (_savesModule == null) {
                if (!_checked) {
                    GUILayout.Label("Checking...");
                    Check(false);
                    _checked = true;
                } else {
                    GUILayout.Label("Saves not existed");
                    if (EditorGUIUtils.RefreshButton()) {
                        Check(true);
                    }
                }
            } else {
                GUILayout.Label("Saves module created!");
            }
            EditorGUILayout.EndHorizontal();
        }

        private void Check(bool create) {
            _savesModule = DB.Get<ModuleDBEntry>(_moduleName);
            if (create) {
                Modules.Editor.ModulesEditorUtils.CreateModule<Saves>(_moduleName, saves => saves.defaultMethod = DB.Get<ToTXTFile>());
            }
        }
    }
}