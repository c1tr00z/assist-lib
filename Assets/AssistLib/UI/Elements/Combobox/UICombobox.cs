using System;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;
using UnityEngine.Events;

namespace c1tr00z.AssistLib.UI {
    public class UICombobox : DataModelBase {

        #region Private Fields

        private RectTransform _rectTransform;

        private Transform _cancelObjectParent;
        
        private Transform _optionsObjectParent;

        #endregion

        #region Serialized Fields

        [SerializeField] private UnityEvent _onSelected;

        [SerializeField] private RectTransform _cancelObject;

        [SerializeField] private RectTransform _optionsObject;

        [ReferenceTypeAttribute(typeof(object))]
        [SerializeField]
        private PropertyReference _valueSrc;

        #endregion

        #region Accessors

        public object selectedValue { get; private set; }
        
        public bool isShowOptions { get; private set; }
        
        public bool isShowCurrentValue { get; private set; }

        #endregion

        public RectTransform rectTransform {
            get { return this.GetCachedComponent(ref _rectTransform); }
        }

        private void Start() {
            UpdateControls();
            OnSelected();
        }

        public void OnSelected() {
            selectedValue = _valueSrc.Get<object>();
            _onSelected?.Invoke();
            UpdateControls();
            ResetControls();
        }

        public void ResetControls() {
            isShowOptions = false;
            isShowCurrentValue = !isShowOptions;
            UpdateControls();
            AttachControls();
            OnDataChanged();
        }

        public void ShowOptions() {
            isShowOptions = true;
            isShowCurrentValue = !isShowOptions;
            UpdateControls();
            DetachControls();
            OnDataChanged();
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
        }
    }
}
