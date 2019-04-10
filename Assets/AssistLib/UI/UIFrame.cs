using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(RectTransform))]
    public class UIFrame : MonoBehaviour {

        private RectTransform _rectTransform;

        public RectTransform rectTransform {
            get {
                if (_rectTransform == null) {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }
    }
}