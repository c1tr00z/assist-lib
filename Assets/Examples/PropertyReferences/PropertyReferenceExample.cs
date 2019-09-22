using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
	public class PropertyReferenceExample : MonoBehaviour {

		[ReferenceType(typeof(string))]
		public PropertyReference test;

	}
}