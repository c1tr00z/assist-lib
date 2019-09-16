using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(RectTransform))]
    public abstract class UILayerBase : MonoBehaviour {

        private RectTransform _rectTransform;
        private Canvas _canvas;

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

        public Canvas canvas {
            get {
                if (_canvas == null) {
                    _canvas = GetComponent<Canvas>();
                }
                return _canvas;
            }
        }

        public UILayerDBEntry layerDBEntry {
            get { return _layerDBEntry; }
        }

        public bool usedByHotkeys {
            get { return layerDBEntry.usedByHotkeys; }
        }

        public void Init(UILayerDBEntry layerDBEntry) {
            _layerDBEntry = layerDBEntry;
            name = layerDBEntry.name;
            canvas.sortingOrder = layerDBEntry.sortOrder;
        }

        public abstract List<UIFrame> currentFrames { get; }

        public abstract void Show(UIFrameDBEntry frame, params object[] args);

        protected UIFrame ShowFrame(UIFrameDBEntry frameItem, params object[] args) {
            var frame = frameItem.LoadFrame().Clone(rectTransform);
            frame.Show(this);
            frame.rectTransform.Stretch();

            if (args != null && args.Length > 0) {
                frame.SendMessage("OnShowParams", args, SendMessageOptions.DontRequireReceiver);
            } else {
                frame.SendMessage("OnShow", SendMessageOptions.DontRequireReceiver);
            }
            return frame;
        }

        public abstract void Close(UIFrameDBEntry frameDBEntry);

        public void CloseAll() {
            currentFrames.ForEach(f => Close(f.GetComponent<DBEntryResource>().parent as UIFrameDBEntry));
        }
    }
}
