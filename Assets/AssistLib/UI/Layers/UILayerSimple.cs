using System.Collections.Generic;

namespace c1tr00z.AssistLib.UI {
    public class UILayerSimple : UILayerBase {

        public List<UIFrame> currentFrames { get; private set; }

        public override void Close(UIFrameDBEntry frameDBEntry) {
            var frames = currentFrames.Where(f => f.GetComponent<DBEntryResource>().parent == frameDBEntry).ToList();
            currentFrames.RemoveAll(f => frames.Contains(f));
            frames.ForEach(f => Destroy(f));
        }

        public override void Show(UIFrameDBEntry frameDBEntry, params object[] args) {
            currentFrames.Add(ShowFrame(frameDBEntry, args));
        }
    }
}
