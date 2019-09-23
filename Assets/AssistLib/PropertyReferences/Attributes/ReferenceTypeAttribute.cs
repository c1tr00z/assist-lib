using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
	public class ReferenceTypeAttribute : PropertyAttribute {

		public System.Type type { get; private set; }

		public ReferenceTypeAttribute (System.Type type) {
			this.type = type;
		}

		public override bool Match (object obj) {
			var other = obj as ReferenceTypeAttribute;
			if (other == null) {
				return false;
			}

			return other.type == this.type;
		}
	}
}
