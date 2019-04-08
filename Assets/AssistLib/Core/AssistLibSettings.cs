using UnityEngine;
using System.Collections;

public class AssistLibSettings {

    public static string editorSettingsKey {
        get {
            return "EditorSettings:";
        }
    }

    public static string localizationDocumentKey {
        get {
            return PlayerPrefs.GetString(editorSettingsKey + "LocalizationDocumentKey");
        }
        set {
            PlayerPrefs.SetString(editorSettingsKey + "LocalizationDocumentKey", value);
        }
    }

    public static string localizationDocumentFileExtension {
        get {
            return PlayerPrefs.GetString(editorSettingsKey + "LocalizationDocumentFileExtension");
        }
        set {
            PlayerPrefs.SetString(editorSettingsKey + "LocalizationDocumentFileExtension", value);
        }
    }

    public static string language {
        get {
            return PlayerPrefs.GetString(editorSettingsKey + "Language");
        }
        set {
            PlayerPrefs.SetString(editorSettingsKey + "Language", value);
        }
    }

    public static void Save() {
        PlayerPrefs.Save();
    }
}
