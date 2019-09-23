using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public interface IDataModelBase {
        void OnDataChanged();
        void AddReceiver(IValueReceiver receiver);
    }
}