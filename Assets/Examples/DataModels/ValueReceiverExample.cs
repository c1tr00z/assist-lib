using System.Collections.Generic;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
	public class ValueReceiverExample : ValueReceiverBase {

		[ReferenceTypeAttribute(typeof(string))]
		[SerializeField]
		private PropertyReference _reference;

		private string _value = "ABC";

		public override IEnumerator<PropertyReference> GetReferences () {
			yield return _reference;
		}

		public override void UpdateReceiver () {
			_value = _reference.Get<string>();
			Debug.Log(_value);
		}
	}
}