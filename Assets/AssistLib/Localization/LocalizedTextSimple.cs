using UnityEngine;

namespace c1tr00z.AssistLib.Localization {
    public class LocalizedTextSimple : LocalizedText {

        protected override string GetLocalizedText() {
            return Localization.Translate(key);
        }

    }
}
