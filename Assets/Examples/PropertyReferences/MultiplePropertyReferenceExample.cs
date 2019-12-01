using System;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.Localization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace c1tr00z.AssistLib.PropertyReferences {
    public class MultiplePropertyReferenceExample : DataModelBase {
        public string string1 { get; private set; }
        public string string2 { get; private set; }
        public string string3 { get; private set; }
        public string string4 { get; private set; }
        public string string5 { get; private set; }

        private void Start() {
            Refresh();
        }

        private void Update() {
            if (Input.GetKeyUp(KeyCode.R)) {
                Refresh();
            }
        }

        private void Refresh() {
            string1 = StringUtils.RandomString(Random.Range(5, 25));
            string2 = StringUtils.RandomString(Random.Range(5, 25));
            string3 = StringUtils.RandomString(Random.Range(5, 25));
            string4 = StringUtils.RandomString(Random.Range(5, 25));
            string5 = StringUtils.RandomString(Random.Range(5, 25));
            OnDataChanged();
        }
    }
}
