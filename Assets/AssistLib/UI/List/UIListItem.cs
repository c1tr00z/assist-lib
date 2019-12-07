using UnityEngine;
using System.Collections;

namespace c1tr00z.AssistLib.UI {
    public class UIListItem : MonoBehaviour {

        public object item { get; private set; }
        
        public UIList list { get; private set; }
        
        public bool isSelected { get; private set; }

        public void Init(UIList list) {
            this.list = list;
        }

        public virtual void UpdateItem(object item) {
            if (this.item != item) {
                this.item = item;
                GetComponents<IUIListItemView>().ForEach(listItem => listItem.UpdateItem(item));
            }
        }

        public void Select() {
            list.Select(this);
        }

        public void SetSelected(bool selected) {
            if (selected == isSelected) {
                return;
            }
            isSelected = selected;
            GetComponents<IUIListItemView>().ForEach(listItem => listItem.UpdateView());
        }
    }
}