using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.Localization {
    [RequireComponent(typeof(Text))]
    public class LocalizedTextRandomSimple : LocalizedText {
        protected override string GetLocalizedText() {
            return Localization.TranslateRandom(key);
        }
    }
}
