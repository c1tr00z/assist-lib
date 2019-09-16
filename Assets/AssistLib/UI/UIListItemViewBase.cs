using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public abstract class UIListItemViewBase<T> : MonoBehaviour, IUIListItemView {

        public T item { get; protected set; }

        public void UpdateItem(object item) {
            this.item = (T)item;
            if (this.item != null) {
                UpdateView();
            }
        }

        protected abstract void UpdateView();
    }
}