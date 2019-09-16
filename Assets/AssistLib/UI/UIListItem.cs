using UnityEngine;
using System.Collections;

namespace c1tr00z.AssistLib.UI {
    public class UIListItem : MonoBehaviour {

        public object item { get; private set; }

        public virtual void UpdateItem(object item) {

            if (this.item != item) {
                this.item = item;
                GetComponents<IUIListItemView>().ForEach(listItem => listItem.UpdateItem(item));
            }
        }
    }
}