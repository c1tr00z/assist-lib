using c1tr00z.AssistLib.EditorTools;
using c1tr00z.AssistLib.GoogleSpreadsheetImporter;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace c1tr00z.AssistLib.Localization {
    [EditorToolName("Localization tool")]
    public class LocalizationEditorTool : EditorTool {

        private static string PAGES_KEY = "pages";

        private List<GoogleSpreadsheetDocumentPageDBEntry> _pages = new List<GoogleSpreadsheetDocumentPageDBEntry>();

        public override void Init(Dictionary<string, object> settings) {
            base.Init(settings);
            _pages = settings.GetIEnumerable<string>(PAGES_KEY).Select(pageName => DB.Get<GoogleSpreadsheetDocumentPageDBEntry>(pageName)).ToList();
        }

        public override void Save(Dictionary<string, object> settings) {
            base.Save(settings);
            settings.AddOrSet(PAGES_KEY, _pages.SelectNotNull().SelectNotNull(p => p.name).ToArray());
        }

        protected override void DrawInterface() {

            EditorGUILayout.BeginVertical();

            if (Button("+")) {
                _pages.Add(null);
            }

            for (var i = 0; i < _pages.Count; i++) {
                EditorGUILayout.BeginHorizontal();
                _pages[i] = (GoogleSpreadsheetDocumentPageDBEntry)EditorGUILayout.ObjectField(_pages[i], typeof(GoogleSpreadsheetDocumentPageDBEntry), false);
                if (Button("-", GUILayout.Width(50))) {
                    RemoveIndex(i);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();

            if (Button("Get localizations")) {
                var allLocalizations = new Dictionary<string, Dictionary<string, string>>();
                _pages.ForEach(p => {
                    var pageLocalizations = GoogleSpreadsheetDocumentImpoter.Import(p);
                    pageLocalizations.Keys.ForEach(language => {
                        if (allLocalizations.ContainsKey(language)) {
                            pageLocalizations[language].Keys.ForEach(key => allLocalizations[language].AddOrSet(key, pageLocalizations[language][key]));
                        } else {
                            allLocalizations.AddOrSet(language, pageLocalizations[language]);
                        }
                    });
                });
                StoreLocalization(allLocalizations);
            }

            EditorGUILayout.EndHorizontal();
        }

        private void RemoveIndex(int index) {
            _pages.RemoveAt(index);
        }

        private void StoreLocalization(Dictionary<string, Dictionary<string, string>> localization) {
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in localization) {
                var lang = ItemsEditor.CreateOrGetItem<LanguageItem>(PathUtils.Combine("Assets", "Localization", "Resources", "Languages"), kvp.Key);
                var json = JSONUtuls.Serialize(kvp.Value).DecodeEncodedNonAscii();
                var file = new FileInfo(PathUtils.Combine(Application.dataPath, "Localization", "Resources", "Languages", kvp.Key + "@text.txt"));
                if (!file.Exists) {
                    file.Create().Close();
                }
                Debug.Log(file.ToString());
                using (StreamWriter sw = new StreamWriter(file.ToString())) {
                    sw.Write(json);
                    sw.Close();
                }
            }

            Debug.Log(JSONUtuls.Serialize(localization).DecodeEncodedNonAscii());
        }
    }
}