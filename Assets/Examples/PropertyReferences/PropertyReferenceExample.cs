using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
	public class PropertyReferenceExample : MonoBehaviour {

		[ReferenceType(typeof(Transform))]
		public PropertyReference test;

        public string foo;
	}
}