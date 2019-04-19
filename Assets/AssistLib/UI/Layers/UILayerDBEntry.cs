using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UILayerDBEntry : DBEntry {
        [SerializeField] private int _sortOrder;

        [SerializeField] private bool _usedByHotkeys = true;

        public int sortOrder {
            get { return _sortOrder; }
        }

        public bool usedByHotkeys {
            get { return _usedByHotkeys; }
        }
    }
}
