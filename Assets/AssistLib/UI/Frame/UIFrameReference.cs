using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(RectTransform))]
    public class UIFrameReference : MonoBehaviour {

        public UIFrameDBEntry frameDBEntry;

        public bool stretch;

        private UIFrame _currentFrame;

        private void Start() {
            RespawnFrame();
        }

        private void RespawnFrame() {
            if (_currentFrame != null) {
                Destroy(_currentFrame.gameObject);
            }
            _currentFrame = frameDBEntry.LoadPrefab<UIFrame>().Clone(transform);

            if (stretch) {
                _currentFrame.rectTransform.Stretch();
            }
        }
    }
}