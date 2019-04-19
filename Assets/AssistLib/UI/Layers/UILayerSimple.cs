using System.Collections.Generic;

namespace c1tr00z.AssistLib.UI {
    public class UILayerSimple : UILayerBase {

        private List<UIFrame> _currentFrames = new List<UIFrame>();

        public override List<UIFrame> currentFrames {
            get { return _currentFrames; }
        }

        public override void Close(UIFrameDBEntry frameDBEntry) {
            var frames = currentFrames.Where(f => f.GetComponent<DBEntryResource>().parent == frameDBEntry).ToList();
            _currentFrames.RemoveAll(f => frames.Contains(f));
            frames.ForEach(f => Destroy(f.gameObject));
        }

        public override void Show(UIFrameDBEntry frameDBEntry, params object[] args) {
            _currentFrames.Add(ShowFrame(frameDBEntry, args));
        }
    }
}
