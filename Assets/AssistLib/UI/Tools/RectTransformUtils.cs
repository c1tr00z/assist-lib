using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public static class RectTransformUtils {
        
        public static void Stretch(this RectTransform rectTransform) {
            rectTransform.localScale = Vector3.one;
            rectTransform.rect.Set(0, 0, 0, 0);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}