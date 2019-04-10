using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UIDefaultsDBEntry : DBEntry {

        [SerializeField]
        private UILayerDBEntry _mainLayer;

        [SerializeField]
        private UILayerDBEntry _defaultLayer;

        public UILayerDBEntry mainLayer {
            get { return _mainLayer; }
        }

        public UILayerDBEntry defaultLayer {
            get { return _defaultLayer; }
        }
    }
}