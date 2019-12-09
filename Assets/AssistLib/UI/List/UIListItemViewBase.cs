using c1tr00z.AssistLib.DataModels;

namespace c1tr00z.AssistLib.UI {
    public abstract class UIListItemViewBase<T> : DataModelBase, IUIListItemView {

        private UIListItem _listItem;

        public T item { get; protected set; }

        protected UIListItem listItem {
            get { return this.GetCachedComponent(ref _listItem); }
        }

        public void UpdateItem(object item) {
            this.item = (T)item;
            if (this.item != null) {
                UpdateView();
            }
        }

        public void UpdateView() {
            OnDataChanged();
        }
    }
}