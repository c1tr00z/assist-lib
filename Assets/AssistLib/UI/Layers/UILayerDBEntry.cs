using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UILayerDBEntry : DBEntry {
        [SerializeField] private int _sortOrder;

        public int sortOrder {
            get { return _sortOrder; }
        }
    }
}
