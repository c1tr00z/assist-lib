using c1tr00z.AssistLib.GoogleSpreadsheetImporter;
using UnityEditor;

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
