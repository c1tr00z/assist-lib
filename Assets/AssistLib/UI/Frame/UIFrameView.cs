using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.UI;

namespace DefaultNamespace {
    public class UIFrameView : DataModelBase, IUIFrameView {
        public virtual void OnShow(params object[] args) {
            OnDataChanged();
        }
    }
}