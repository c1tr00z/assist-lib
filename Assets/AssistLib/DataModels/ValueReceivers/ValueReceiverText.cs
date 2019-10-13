using System.Collections.Generic;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.DataModels {
    [RequireComponent(typeof(Text))]
    public class ValueReceiverText : ValueReceiverBase {
        [ReferenceType(typeof(string))]
        [SerializeField]
        private PropertyReference _textSource;

        private Text _text;

        public Text text {
            get {
                if (_text == null) {
                    _text = GetComponent<Text>();
                }

                return _text;
            }
        }

        public override void UpdateReceiver() {
            text.text = _textSource.Get<string>();
        }
        
        public override IEnumerator<PropertyReference> GetReferences() {
            yield return _textSource;
        }
    }
}