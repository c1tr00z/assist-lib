using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public class DataModelBase : MonoBehaviour, IDataModelBase {

        public virtual bool isDataModelEnabled {
            get { return true; }
        }

        private List<IValueReceiver> _valueReceivers = new List<IValueReceiver>();

        public void AddReceiver(IValueReceiver receiver) {
            if (_valueReceivers.Contains(receiver)) {
                return;
            }
            _valueReceivers.Add(receiver);
        }

        public void RemoveReceiver(IValueReceiver receiver) {
            if (!_valueReceivers.Contains(receiver)) {
                return;
            }

            _valueReceivers.Remove(receiver);
        }


        public void OnDataChanged() {
            if (_valueReceivers.Count == 0) {
                return;
            }
            _valueReceivers.Where(r => r.isRecieverEnabled).ForEach(r => r.UpdateReceiver());
        }
    }
}