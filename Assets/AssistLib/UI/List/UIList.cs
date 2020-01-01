using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.UI {
[RequireComponent(typeof(LayoutGroup))]
    public class UIList : MonoBehaviour {

        [SerializeField] private UIListItemDBEntry listItemDBEntry;
        
        [SerializeField] private bool _useSelect;

        [SerializeField] private bool _allowReselectSame;

        [SerializeField] private UnityEvent _onSelected;

        private List<UIListItem> _listItems;

        public object selectedValue { get; private set; }

        public void UpdateList<T>(IEnumerable<T> items, T selectedItem = default(T)) {
            if (_listItems != null) {
                foreach (var listItem in _listItems) {
                    Destroy(listItem.gameObject);
                }
            }

            _listItems = new List<UIListItem>();

            foreach (var item in items) {
                CreateListItem(item);
            }

            if (_useSelect && (selectedItem != null && !selectedItem.Equals(default(T))) && _listItems.Count > 0 &&!_listItems.Any(li => li.isSelected)) {
                Select(_listItems.FirstOrDefault());
            }
        }

        private UIListItem CreateListItem(object item) {
            var listItem = listItemDBEntry.LoadPrefab<UIListItem>().Clone();
            listItem.transform.SetParent(transform, false);
            listItem.transform.localScale = Vector3.one;

            var rectTransform = listItem.transform as RectTransform;

            listItem.Init(this);
            listItem.UpdateItem(item);
            _listItems.Add(listItem);
            return listItem;
        }

        public void Select(UIListItem item) {
            if ((!_allowReselectSame && item.isSelected) || !_listItems.Contains(item)) {
                return;
            }
            
            _listItems.ForEach(li => li.SetSelected(li == item));

            selectedValue = item.item;
            
            _onSelected?.Invoke();
        }
    }
}