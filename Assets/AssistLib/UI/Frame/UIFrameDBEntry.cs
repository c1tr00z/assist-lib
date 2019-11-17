using UnityEngine;
using System.Collections;

namespace c1tr00z.AssistLib.UI {
    public class UIFrameDBEntry : DBEntry {

        [SerializeField]
        private UILayerDBEntry _layer;

        public UILayerDBEntry layer {
            get { return _layer; }
        }

        public UIFrame LoadFrame() {
            return this.LoadPrefab<UIFrame>();
        }

        public void Show() {
            UI.instance.Show(this);
        }
    }
}
