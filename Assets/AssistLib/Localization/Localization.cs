using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace c1tr00z.AssistLib.Localization {
    public class Localization {

        private const SystemLanguage _defaultSystemLanguage = SystemLanguage.English;
        private static SystemLanguage _currentSystemLanguage = Application.systemLanguage;
        private static LanguageItem _defaultLanguage;
        private static LanguageItem _currentLanguage;

        private static bool _inited = false;

        private static Dictionary<string, string> _translations;

        public static System.Action<LanguageItem> changeLanguage = (language) => { };

        public static string Translate(string key) {

            var translation = key;

            if (!_inited) {
                _defaultLanguage = DB.Get<LanguageItem>(_defaultSystemLanguage.ToString());

                if (string.IsNullOrEmpty(AssistLibSettings.language)) {
                    ChangeLanguage(_currentSystemLanguage.ToString());
                } else {
                    ChangeLanguage(AssistLibSettings.language);
                }

                _inited = true;
            }

            if (_defaultLanguage != null && _defaultLanguage.translations != null && _defaultLanguage.translations.ContainsKey(key)) {
                translation = _defaultLanguage.translations[key];
            }

            if (_currentLanguage != null && _currentLanguage.translations != null && _currentLanguage.translations.ContainsKey(key)) {
                translation = _currentLanguage.translations[key];
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
            if (string.IsNullOrEmpty(AssistLibSettings.language)) {
                AssistLibSettings.language = _currentSystemLanguage.ToString();
                AssistLibSettings.Save();
            }
            _currentLanguage = DB.Get<LanguageItem>(AssistLibSettings.language);

            if (_inited) {
                changeLanguage(newLanguage);
            }
        }
    }
}
