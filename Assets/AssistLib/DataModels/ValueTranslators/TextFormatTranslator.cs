using System.Collections.Generic;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {

    public class TextFormatTranslator : DataTranslator {

        [ReferenceType(typeof(string))] [SerializeField]
        private PropertyReference[] _textSources;

        [SerializeField] private string _format;
        
        public string text { get; private set; }

        public override void UpdateReceiver() {
            var formatParams = _textSources.Select(s => s.Get<string>()).ToArray();
            text = string.Format(_format, formatParams);
            OnDataChanged();
        }

        public override IEnumerator<PropertyReference> GetReferences() {
            foreach (var source in _textSources) {
                yield return source;
            }
        }
    }
}
