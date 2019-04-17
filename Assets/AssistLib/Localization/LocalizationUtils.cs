namespace c1tr00z.AssistLib.Localization {
    public static class LocalizatioUtils {

        private static string KEY_TITLE = "Title";
        private static string KEY_DESCRIPTION = "Description";

        public static string GetLocalizationText(this DBEntry dBEntry, string key) {
            return Localization.Translate(string.Format("{0}@{1}", dBEntry.name, key));
        }

        public static string GetLocalizationText(this DBEntry dBEntry, string key, params object[] localizationParams) {
            return string.Format(GetLocalizationText(dBEntry, key), localizationParams);
        }

        public static string GetTitle(this DBEntry dBEntry) {
            return GetLocalizationText(dBEntry, KEY_TITLE);
        }

        public static string GetDescription(this DBEntry dBEntry) {
            return GetLocalizationText(dBEntry, KEY_DESCRIPTION);
        }
    }
}