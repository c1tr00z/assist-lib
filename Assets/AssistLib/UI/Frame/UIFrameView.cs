using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.UI;

namespace DefaultNamespace {
    public class UIFrameView : DataModelBase, IUIFrameView {
        public void OnShow(params object[] args) {
            OnViewShow(args);
            OnDataChanged();
        }

        protected virtual void OnViewShow(params object[] args) { }
    }
}