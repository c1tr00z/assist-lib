namespace c1tr00z.AssistLib.Localization {
    public static class LocalizatioUtils {

        private static string KEY_TITLE = "Title";
        private static string KEY_DESCRIPTION = "Description";

        public static string GetLocalizationText(this string key) {
            return key.GetLocalizationText(false);
        }
        
        public static string GetLocalizationText(this string key, bool random) {
            return random ? Localization.TranslateRandom(key) : Localization.Translate(key);
        }
        
        public static string GetLocalizationText(this string key, bool random, params object[] localizationParams) {
            return string.Format(GetLocalizationText(key, random), localizationParams);
        }
        
        public static string GetLocalizationText(this string key, params object[] localizationParams) {
            return key.GetLocalizationText(false, localizationParams);
        }

        public static string GetLocalizationText(this DBEntry dBEntry, bool random, string key) {
            return $"{dBEntry.name}@{key}".GetLocalizationText(random);
        }
        
        public static string GetLocalizationText(this DBEntry dBEntry, string key) {
            return dBEntry.GetLocalizationText(key, false);
        }

        public static string GetLocalizationText(this DBEntry dBEntry, string key, bool random, params object[] localizationParams) {
            return string.Format($"{dBEntry.name}@{key}".GetLocalizationText(random), localizationParams);
        }
        
        public static string GetLocalizationText(this DBEntry dBEntry, string key, params object[] localizationParams) {
            return dBEntry.GetLocalizationText(key, false, localizationParams);
        }

        public static string GetTitle(this DBEntry dBEntry) {
            return GetLocalizationText(dBEntry, KEY_TITLE);
        }

        public static string GetDescription(this DBEntry dBEntry) {
            return GetLocalizationText(dBEntry, KEY_DESCRIPTION);
        }
    }
}