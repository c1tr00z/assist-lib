using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UILayerSingleFramed : UILayerBase {

        private UIFrame _currentFrame;

        private UIFrameDBEntry _currentFrameDBEntry;

        public override void Close(UIFrameDBEntry frameDBEntry) {
            if (_currentFrameDBEntry == frameDBEntry) {
                Destroy(_currentFrame);
                _currentFrameDBEntry = null;
            }
        }

        public override void Show(UIFrameDBEntry frame, params object[] args) {
            if (_currentFrameDBEntry == frame) {
                return;
            }
            var prevFrame = _currentFrame;
            _currentFrameDBEntry = frame;
            _currentFrame = ShowFrame(frame, args);
            Destroy(prevFrame);
        }
    }
}