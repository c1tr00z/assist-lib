using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(RectTransform))]
    [ExecuteInEditMode]
    public class UIScreenStretch : MonoBehaviour {

        [SerializeField] private bool _stretchHorizontal;
        [SerializeField] private bool _stretchVertical;

        private RectTransform _rectTransform;
        private CanvasScaler _canvasScaler;

        public RectTransform rectTransform {
            get { return this.GetCachedComponent(ref _rectTransform); }
        }

        public CanvasScaler canvasScaler {
            get { return this.GetCachedComponentInParent(ref _canvasScaler); }
        }

        private void Update() {

            if (!_stretchHorizontal && !_stretchVertical) {
                return;
            }
            
            if (_canvasScaler == null) {
                _canvasScaler = GetComponentInParent<CanvasScaler>();
            }

            if (canvasScaler == null) {
                return;
            }
            
            if (_rectTransform == null) {
                _rectTransform = GetComponentInParent<RectTransform>();
            }

            var scale = canvasScaler.transform.localScale;
            
            
            rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, transform.position.z);

            if (_stretchHorizontal) {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 
                    Screen.width  / canvasScaler.transform.localScale.x);
            }

            if (_stretchVertical) {
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                    Screen.height / canvasScaler.transform.localScale.y);
            }
        }
    }
}