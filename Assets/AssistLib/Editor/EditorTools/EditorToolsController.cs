using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace c1tr00z.AssistLib.EditorTools {
    public class EditorToolsController {

        private static string _editorToolsSettingsKey = "EditorTools";

        private Dictionary<Type, EditorTool> tools = new Dictionary<Type, EditorTool>();

        public EditorToolsController() {
            var settingsHash = AssistLibEditorSettings.GetDataNode(_editorToolsSettingsKey);

            AppDomain.CurrentDomain.GetAssemblies().ForEach(assembly => {
                assembly.GetTypes().Where(t => t.IsSubclassOf(typeof(EditorTool)) && !t.IsAbstract).ForEach(t => {
                    if (!tools.ContainsKey(t)) {
                        var tool = (EditorTool)Activator.CreateInstance(t);
                        tools.Add(t, tool);
                        tool.Init(settingsHash.GetChild(t.ToString()));
                    }
                });
            });
        }

        public void DrawTools() {
            tools.Values.ForEach(tool => tool.Draw());
        }

        public void SaveTools() {
            var settingsHash = AssistLibEditorSettings.GetDataNode(_editorToolsSettingsKey);
            tools.Values.ForEach(tool => {
                var toolSettings = settingsHash.GetChild(tool.GetType().ToString());
                tool.Save(toolSettings);
                settingsHash.AddOrSet(tool.GetType().ToString(), toolSettings);
            });
            AssistLibEditorSettings.SetDataNode(_editorToolsSettingsKey, settingsHash);
        }
    }
}
