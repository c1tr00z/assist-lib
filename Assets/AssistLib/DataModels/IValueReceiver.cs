using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public interface IValueReceiver {
        
        bool isRecieverEnabled { get; }
        
        void UpdateReceiver();

        IEnumerable<IDataModelBase> GetModels();
    }
}
