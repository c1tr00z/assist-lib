using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public interface IValueReceiver {
        void UpdateReceiver();

        IEnumerable<IDataModelBase> GetModels();
    }
}
