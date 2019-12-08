using c1tr00z.AssistLib.DataBase.Editor;
using c1tr00z.AssistLib.GoogleSpreadsheetImporter;
using UnityEditor;

namespace c1tr00z.AssistLib.Localization.Editor {
    public static class LocalizationEditor {
        [MenuItem("Assets/AssistLib/Create Google Spreadsheet Document")]
        public static void CreateSpreadsheetDocument() {
            ItemsEditor.CreateItem<GoogleSpreadsheetDocumentDBEntry>();
        }

        [MenuItem("Assets/AssistLib/Create Google Spreadsheet Document Page")]
        public static void CreateSpreadsheetDocumentPage() {
            ItemsEditor.CreateItem<GoogleSpreadsheetDocumentPageDBEntry>();
        }
    }
}
