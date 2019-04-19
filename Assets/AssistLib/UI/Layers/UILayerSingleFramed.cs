using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UILayerSingleFramed : UILayerBase {

        private UIFrame _currentFrame;

        private UIFrameDBEntry _currentFrameDBEntry;

        public override List<UIFrame> currentFrames {
            get {
                return new List<UIFrame> { _currentFrame };
            }
        }

        public override void Close(UIFrameDBEntry frameDBEntry) {
            if (_currentFrameDBEntry == frameDBEntry) {
                Destroy(_currentFrame.gameObject);
                _currentFrameDBEntry = null;
            }
        }

        private void Close(UIFrame frame) {
            if (frame == _currentFrame) {
                Close(_currentFrameDBEntry);
            }
        }

        public override void Show(UIFrameDBEntry frame, params object[] args) {
            if (_currentFrameDBEntry == frame) {
                return;
            }
            var prevFrame = _currentFrame;
            _currentFrameDBEntry = frame;
            _currentFrame = ShowFrame(frame, args);
            if (prevFrame != null) {
                Destroy(prevFrame.gameObject);
            }
        }
    }
}