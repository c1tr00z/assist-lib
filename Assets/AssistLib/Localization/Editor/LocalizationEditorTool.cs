using c1tr00z.AssistLib.GoogleSpreadsheetImporter;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace c1tr00z.AssistLib.Localization {
    [EditorToolName("Localization tool")]
    public class LocalizationEditorTool : GoogleSpreadsheetDocumentImportEditorTool {
        protected override void ProcessImport() {
            var allLocalizations = new Dictionary<string, Dictionary<string, string>>();
            pages.ForEach(p => {
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

        private void StoreLocalization(Dictionary<string, Dictionary<string, string>> localization) {
            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in localization) {
                var lang = DB.Get<LanguageItem>(kvp.Key);
                if (lang == null) {
                    lang = AssetDBUtils.CreateScriptableObject<LanguageItem>(PathUtils.Combine("Assets", "Localization", "Resources", "Languages"), kvp.Key);
                }
                var json = JSONUtuls.Serialize(kvp.Value).DecodeEncodedNonAscii();
                FileUtils.SaveTextToFile(PathUtils.Combine(Application.dataPath, "Localization", "Resources", "Languages", kvp.Key + "@text.txt"), json);
            }

            Debug.Log(JSONUtuls.Serialize(localization).DecodeEncodedNonAscii());
        }
    }
}