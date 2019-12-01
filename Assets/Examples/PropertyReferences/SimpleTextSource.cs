using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.Localization;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
    public class SimpleTextSource : DataModelBase {
        public string text { get; private set; }

        public override bool isDataModelEnabled {
            get { return !string.IsNullOrEmpty(text); }
        }

        private void Start() {
            Refresh();
        }

        private void Update() {
            if (Input.GetKeyUp(KeyCode.R)) {
                Refresh();
            }
        }
        
        private void Refresh() {
            text = StringUtils.RandomString(Random.Range(5, 25));
            OnDataChanged();
        }
    }
}
