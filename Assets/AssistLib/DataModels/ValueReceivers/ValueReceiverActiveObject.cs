using System.Collections;
using System.Collections.Generic;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {

    public class ValueReceiverActiveObject : ValueReceiverBase {

        [ReferenceTypeAttribute(typeof(bool))]
        [SerializeField] private PropertyReference _isActiveSrc;
        [SerializeField] private GameObject _target;
        
        public override void UpdateReceiver() {
            _target.SetActive(_isActiveSrc.Get<bool>());
        }

        public override IEnumerator<PropertyReference> GetReferences() {
            yield return _isActiveSrc;
        }
    }
}