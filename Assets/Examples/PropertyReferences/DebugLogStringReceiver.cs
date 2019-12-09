using System.Collections.Generic;
using c1tr00z.AssistLib.DataModels;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;

public class DebugLogStringReceiver : ValueReceiverBase {
    
    [ReferenceType(typeof(string))]
    public PropertyReference textSource;
    
    public override IEnumerator<PropertyReference> GetReferences() {
        yield return textSource;
    }

    public override void UpdateReceiver() {
        Debug.Log(textSource.Get<string>());
    }
}
