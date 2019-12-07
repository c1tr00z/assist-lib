using System.Collections;
using System.Collections.Generic;
using c1tr00z.AssistLib.PropertyReferences;
using c1tr00z.AssistLib.UI;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {

    public class ValueReceiverList : ValueReceiverBase {

        [ReferenceTypeAttribute(typeof(List<>))]
        [SerializeField]
        private PropertyReference _listSource;

        [SerializeField] private UIList _list;
        
        public override IEnumerator<PropertyReference> GetReferences() {
            yield return _listSource;
        }

        public override void UpdateReceiver() {
            var iList = (IList)_listSource.Get<object>();
            var list = new List<object>();
            var listEnum = iList.GetEnumerator();
            while (listEnum.MoveNext()) {
                list.Add(listEnum.Current);
            }
            _list.UpdateList(list);
        }
    }
}

