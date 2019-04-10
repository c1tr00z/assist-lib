using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.UI {
    public class UI : BehaviourSingleton<UI> {

        private Dictionary<UILayerDBEntry, UILayerBase> _layers = new Dictionary<UILayerDBEntry, UILayerBase>();

        private UIDefaultsDBEntry _uiDefaults;

        private UILayerBase _defaultLayerSrc;

        private UIDefaultsDBEntry uiDefaults {
            get {
                if (_uiDefaults == null) {
                    _uiDefaults = DB.Get<UIDefaultsDBEntry>();
                }
                return _uiDefaults;
            }
        }

        private UILayerBase defaultLayerSrc {
            get {
                if (_defaultLayerSrc == null) {
                    _defaultLayerSrc = DB.Get<UIDefaultsDBEntry>().defaultLayer.LoadPrefab<UILayerBase>();
                }
                return defaultLayerSrc;
            }
        }

        public void Show(UIFrameDBEntry newFrame) {
            Show(newFrame, null);
        }

        public void Show(UIFrameDBEntry newFrame, params object[] args) {
            var requiredLayer = GetOrCreateLayer(newFrame.layer);
            requiredLayer.Show(newFrame);
        }

        private UILayerBase GetOrCreateLayer(UILayerDBEntry layerDBEntry) {
            if (layerDBEntry == null) {
                return GetOrCreateLayer(uiDefaults.mainLayer);
            }
            if (_layers.ContainsKey(layerDBEntry)) {
                return _layers[layerDBEntry];
            }
            var existedButNotCached = GetComponentsInChildren<UILayerBase>().Where(l => l.layerDBEntry == layerDBEntry).First();
            if (existedButNotCached != null) {
                _layers.AddOrSet(layerDBEntry, existedButNotCached);
                return existedButNotCached;
            }
            return CreateLayer(layerDBEntry);
        }

        private UILayerBase CreateLayer(UILayerDBEntry layerDBEntry) {
            var layerPrefab = layerDBEntry.LoadPrefab<UILayerBase>();
            if (layerPrefab == null) {
                layerPrefab = defaultLayerSrc;
            }
            var layer = layerPrefab.Clone(transform);
            layer.name = layerDBEntry.name;
            layer.GetComponent<Canvas>().sortingOrder = layerDBEntry.sortOrder;
            _layers.Add(layerDBEntry, layer);
            transform.SetChildrenSiblingIndex(c => c.GetComponent<Canvas>().sortingOrder);
            return layer;
        }
    }
}