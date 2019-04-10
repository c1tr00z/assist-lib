using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIList : MonoBehaviour {

    public enum StartPoint {
        Center,
        Top,
        Bottom,
        Left,
        Right,
        //TopLeft,
        //TopRight,
        //BottomLeft,
        //BottomRight,
    }

    [SerializeField] private ScrollRect _scrollRect;

    [SerializeField] private StartPoint _listStartPoint;

    [SerializeField] private UIListItem listItemSource;

    [SerializeField] private Vector2 _cellSize;

    [SerializeField] private Vector2 _gridSize;

    [SerializeField] private RectTransform _contentTransform;

    private List<UIListItem> _listItems;

    void Awake() {
        if (_scrollRect == null) {
            _scrollRect = GetComponent<ScrollRect>();
        }
    }

    public void UpdateList<T>(IEnumerable<T> items) {
        _contentTransform = _scrollRect.content.transform as RectTransform;
        if (_listItems != null) {
            foreach (var listItem in _listItems) {
                Destroy(listItem.gameObject);
            }
        }

        _listItems = new List<UIListItem>();

        foreach (var item in items) {
            CreateListItem(item);
        }

        RearrangeItems();
    }

    private UIListItem CreateListItem(object item) {
        var listItem = listItemSource.Clone();
        listItem.transform.parent = _contentTransform;
        listItem.transform.localScale = Vector3.one;

        var rectTransform = listItem.transform as RectTransform;

        listItem.UpdateItem(item);
        _listItems.Add(listItem);
        return listItem;
    }

    private void RearrangeItems() {
        for (int i = 0; i < _listItems.Count; i ++) {
            var listItem = _listItems[i];
            var rectTransform = listItem.transform as RectTransform;

            var newPosition = new Vector2();

            if (_scrollRect.horizontal) {
                newPosition.x = i * _cellSize.x - (_listItems.Count * _cellSize.x * 1.0f / 2) + _cellSize.x * 1.0f / 2;
            } else if (_scrollRect.vertical) {
                newPosition.y = i * _cellSize.y - (_listItems.Count * _cellSize.y * 1.0f / 2) + _cellSize.y * 1.0f / 2;
            }

            rectTransform.localPosition = new Vector3(newPosition.x, newPosition.y, 0);
        }

        if (_scrollRect.horizontal) {
            _contentTransform.sizeDelta = new Vector2(_cellSize.x * _listItems.Count, _cellSize.y);
            _contentTransform.localPosition = new Vector3((_listItems.Count * _cellSize.x * 1.0f / 2) + _cellSize.x * 1.0f / 2, 0);
        } else if (_scrollRect.vertical) {
            _contentTransform.sizeDelta = new Vector2(_cellSize.x, _cellSize.y * _listItems.Count);
            _contentTransform.localPosition = new Vector3(0, (_listItems.Count * _cellSize.x * 1.0f / 2) + _cellSize.x * 1.0f / 2);
        }
    }
}
