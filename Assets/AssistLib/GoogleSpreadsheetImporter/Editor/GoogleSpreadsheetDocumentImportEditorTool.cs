using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.GoogleSpreadsheetImporter {
    public abstract class GoogleSpreadsheetDocumentImportEditorTool : EditorTools.EditorTool {

        private static string PAGES_KEY = "pages";

        protected List<GoogleSpreadsheetDocumentPageDBEntry> pages = new List<GoogleSpreadsheetDocumentPageDBEntry>();

        public override void Init(Dictionary<string, object> settings) {
            base.Init(settings);
            pages = settings.GetIEnumerable<string>(PAGES_KEY).Select(pageName => DB.Get<GoogleSpreadsheetDocumentPageDBEntry>(pageName)).ToList();
        }

        public override void Save(Dictionary<string, object> settings) {
            base.Save(settings);
            settings.AddOrSet(PAGES_KEY, pages.SelectNotNull().SelectNotNull(p => p.name).ToArray());
        }

        protected override void DrawInterface() {

            EditorGUILayout.BeginVertical();

            if (Button("+")) {
                pages.Add(null);
            }

            for (var i = 0; i < pages.Count; i++) {
                EditorGUILayout.BeginHorizontal();
                pages[i] = (GoogleSpreadsheetDocumentPageDBEntry)EditorGUILayout.ObjectField(pages[i], typeof(GoogleSpreadsheetDocumentPageDBEntry), false);
                if (Button("-", GUILayout.Width(50))) {
                    RemoveIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();

            if (Button("Import")) {
                ProcessImport();
            }

            EditorGUILayout.EndHorizontal();
        }

        private void RemoveIndex(int index) {
            pages.RemoveAt(index);
        }

        protected abstract void ProcessImport();
    }
}