using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public interface IUIListItemView {

        void UpdateItem(object item);

        void UpdateView();
    }
}