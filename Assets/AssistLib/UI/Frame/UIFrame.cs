using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(RectTransform))]
    public class UIFrame : MonoBehaviour {

        private RectTransform _rectTransform;

        public UILayerBase layer { get; private set; }

        public RectTransform rectTransform {
            get {
                if (_rectTransform == null) {
                    _rectTransform = GetComponent<RectTransform>();
                }
                return _rectTransform;
            }
        }

        public bool isTopFrame {
            get { return UI.instance.IsTopFrameInStack(this); }
        }

        public void Show(UILayerBase layer, params object[] args) {
            this.layer = layer;
            GetComponentsInChildren<IUIFrameView>().ForEach(view => view.OnShow(args));
        }

        public void Close() {
            layer.Close(GetComponent<DBEntryResource>().parent as UIFrameDBEntry);
        }
    }
}