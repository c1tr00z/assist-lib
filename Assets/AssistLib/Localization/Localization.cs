using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace c1tr00z.AssistLib.Localization {
    public static class Localization {

        private const SystemLanguage _defaultSystemLanguage = SystemLanguage.English;
        private static SystemLanguage _currentSystemLanguage = Application.systemLanguage;
        private static LanguageItem _defaultLanguage;
        public static LanguageItem currentLanguage { get; private set; }

        private static string LOCALIZATION_SETTINGS_KEY = "Localization";
        private static string LOCALIZATION_SAVED_LANGUAGE_KEY = "Localization";

        private static LocalizationSettingsDBEntry _settings;

        private static bool _inited = false;

        public static System.Action<LanguageItem> changeLanguage = (language) => { };

        public static bool isMultipleTranslationsSupported {
            get {
                Init();
                return _settings != null && _settings.supportMultipleTranslations;
            }
        }

        private static void Init() {
            if (!_inited) {
                _settings = DB.Get<LocalizationSettingsDBEntry>();
                
                _defaultLanguage = DB.Get<LanguageItem>(_defaultSystemLanguage.ToString());

                var localizationSettingsData = PlayerPrefsLocalData.GetDataNode(LOCALIZATION_SETTINGS_KEY);
                var savedLanguageString = localizationSettingsData.ContainsKey(LOCALIZATION_SAVED_LANGUAGE_KEY)
                    ? localizationSettingsData.GetString(LOCALIZATION_SAVED_LANGUAGE_KEY)
                    : null;

                if (string.IsNullOrEmpty(savedLanguageString)) {
                    ChangeLanguage(_currentSystemLanguage.ToString());
                } else {
                    var savedLanguage = DB.Get<LanguageItem>(savedLanguageString);
                    if (savedLanguage != null) {
                        ChangeLanguage(savedLanguage);
                    } else {
                        Debug.LogWarning(string.Format("Language not found: {0}", savedLanguageString));
                    }
                }

                _inited = true;
            }
        }

        private static string GetTranslationString(string key) {
            var translation = key;

            Init();

            if (_defaultLanguage != null && _defaultLanguage.translations != null 
                && _defaultLanguage.translations.ContainsKey(key) && !string.IsNullOrEmpty(_defaultLanguage.translations[key])) {
                translation = _defaultLanguage.translations[key];
            }

            if (currentLanguage != null && currentLanguage.translations != null 
                && currentLanguage.translations.ContainsKey(key) && !string.IsNullOrEmpty(currentLanguage.translations[key])) {
                translation = currentLanguage.translations[key];
            }

            return translation;
        }

        public static string Translate(string key) {

            var translation = GetTranslationString(key);

            if (isMultipleTranslationsSupported) {
                translation = translation.Split('|').First();
            }

            return translation;
        }
        
        public static string TranslateRandom(string key) {

            var translation = GetTranslationString(key);

            if (isMultipleTranslationsSupported) {
                translation = translation.Split('|').RandomItem();
            }

            return translation;
        }

        public static void ChangeLanguage(string newLanguageName) {
            var newLanguage = DB.Get<LanguageItem>(newLanguageName);

            if (newLanguage == null) {
                Debug.LogError("Invalid language: " + newLanguageName);
                return;
            }

            ChangeLanguage(newLanguage);
        }

        public static void ChangeLanguage(LanguageItem newLanguage) {
            if (currentLanguage == newLanguage) {
                return;
            }
            var localizationSettingsData = PlayerPrefsLocalData.GetDataNode(LOCALIZATION_SETTINGS_KEY);
            localizationSettingsData.AddOrSet(LOCALIZATION_SAVED_LANGUAGE_KEY, newLanguage.name);
            PlayerPrefsLocalData.SetDataNode(LOCALIZATION_SETTINGS_KEY, localizationSettingsData);
            currentLanguage = newLanguage;

            if (_inited) {
                changeLanguage(newLanguage);
            }
        }
    }
}
