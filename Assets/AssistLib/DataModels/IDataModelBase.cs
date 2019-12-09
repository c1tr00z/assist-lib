using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public interface IDataModelBase {
        bool isDataModelEnabled { get; }
        void OnDataChanged();
        void AddReceiver(IValueReceiver receiver);
        void RemoveReceiver(IValueReceiver receiver);
    }
}