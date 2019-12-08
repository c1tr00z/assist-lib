using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.EditorTools;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.DataBase.Editor {
    [EditorToolName("DB Settings")]
    public class DBSettings : EditorTool {
        [Serializable]
        public class SaveData {
            public bool autoCollect;
        }

        private static string key => typeof(DBSettings).FullName;


        private SaveData _saveData;

        public static bool autoCollect => LoadData().autoCollect;

        public override void Init(Dictionary<string, object> settings) {
            base.Init(settings);
            Load();
        }

        public override void Save(Dictionary<string, object> settings) {
            base.Save(settings);
            Save();
        }

        protected override void DrawInterface() {
            _saveData.autoCollect = EditorGUILayout.Toggle("Use autocollect", _saveData.autoCollect);
        }

        private static SaveData LoadData() {
            var json = EditorPrefs.GetString(key);
            return string.IsNullOrEmpty(json) ? new SaveData() : JsonUtility.FromJson<SaveData>(json);
        }

        private void Load() {
            _saveData = LoadData();
        }

        private void Save() {
            var json = JsonUtility.ToJson(_saveData);
            EditorPrefs.SetString(key, json);
        }
    }
}