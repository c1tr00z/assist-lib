using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(RectTransform))]
    public abstract class UILayerBase : MonoBehaviour {

        private RectTransform _rectTransform;

        [SerializeField]
        private UILayerDBEntry _layerDBEntry;

        public RectTransform rectTransform {
            get {
                if (_rectTransform == null) {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        public UILayerDBEntry layerDBEntry {
            get { return _layerDBEntry; }
        }

        public abstract void Show(UIFrameDBEntry frame, params object[] args);

        protected UIFrame ShowFrame(UIFrameDBEntry frameItem, params object[] args) {
            var frame = frameItem.LoadFrame().Clone(rectTransform);
            Stretch(frame.rectTransform);

            if (args != null && args.Length > 0) {
                frame.SendMessage("OnShowParams", args, SendMessageOptions.DontRequireReceiver);
            } else {
                frame.SendMessage("OnShow", SendMessageOptions.DontRequireReceiver);
            }
            return frame;
        }

        public abstract void Close(UIFrameDBEntry frameDBEntry);

        protected void Stretch(RectTransform rectTransform) {
            rectTransform.localScale = Vector3.one;
            rectTransform.rect.Set(0, 0, 0, 0);
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
        }
    }
}
