using System;
using System.Collections.Generic;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;
using UnityEngine.Events;

namespace c1tr00z.AssistLib.UI {
    public class UICombobox : MonoBehaviour {

        #region Private Fields

        private RectTransform _rectTransform;

        private Transform _cancelObjectParent;
        
        private Transform _optionsObjectParent;

        #endregion

        #region Serialized Fields

        [SerializeField] private UnityEvent _onSelected;

        [SerializeField] private RectTransform _cancelObject;

        [SerializeField] private RectTransform _optionsObject;
        
        [SerializeField] private UIList _optionsList;
        
        [SerializeField] private UIListItem _currentValue;

        #endregion

        #region Accessors

        public object selectedValue { get; private set; }
        
        public bool isShowOptions { get; private set; }

        #endregion

        public RectTransform rectTransform {
            get { return this.GetCachedComponent(ref _rectTransform); }
        }

        private void Start() {
            UpdateControls();
            OnSelected();
        }

        public void UpdateCombobox(List<object> options, object selectedValue) {
            _optionsList.UpdateList(options);
            this.selectedValue = selectedValue;
            UpdateControls();
        }

        public void OnSelected() {
            selectedValue = _optionsList.selectedValue;
            _currentValue.gameObject.SetActive(true);
            _onSelected?.Invoke();
            UpdateControls();
            ResetControls();
        }

        public void ResetControls() {
            isShowOptions = false;
            _currentValue.gameObject.SetActive(!isShowOptions);
            UpdateControls();
            AttachControls();
        }

        public void ShowOptions() {
            isShowOptions = true;
            _currentValue.gameObject.SetActive(!isShowOptions);
            UpdateControls();
            DetachControls();
        }

        private void DetachControls() {
            if (_cancelObject != null) {
                _cancelObjectParent = _cancelObject.parent;
                _cancelObject.SetParent(GetComponentInParent<Canvas>().transform);
                _cancelObject.SetAsLastSibling();
            }

            if (_optionsObject != null) {
                _optionsObjectParent = _optionsObject.parent;
                _optionsObject.SetParent(GetComponentInParent<Canvas>().transform);
                _optionsObject.SetAsLastSibling();
            }
        }
        
        private void AttachControls() {
            if (_cancelObject != null && _cancelObjectParent != null) {
                _cancelObject.SetParent(_cancelObjectParent);
                _cancelObject.SetAsLastSibling();
            }

            if (_optionsObject != null && _optionsObjectParent != null) {
                _optionsObject.SetParent(_optionsObjectParent);
                _optionsObject.SetAsLastSibling();
            }
        }

        private void UpdateControls() {
            if (_cancelObject != null) {
                _cancelObject.gameObject.SetActive(isShowOptions);
            }

            if (_optionsObject != null) {
                _optionsObject.gameObject.SetActive(isShowOptions);
            }
            
            _currentValue.UpdateItem(selectedValue);
        }
    }
}
