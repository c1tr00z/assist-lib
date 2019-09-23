using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
	public class DataModelExample : DataModelBase {
		[SerializeField]
		private string _value;

		public string value {
			get { return _value; }
		}

		private void Start () {
			OnDataChanged();
		}
	}
}