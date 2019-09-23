using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public class DataModelBase : MonoBehaviour, IDataModelBase {

        private List<IValueReceiver> _valueReceivers = new List<IValueReceiver>();

        public void AddReceiver(IValueReceiver receiver) {
            _valueReceivers.Add(receiver);
        }

        public void OnDataChanged() {
            _valueReceivers.ForEach(r => r.UpdateReceiver());
        }
    }
}