using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
	[System.Serializable]
    public class PropertyReference {

        public GameObject target;

        public string referenceString;

		public System.Type targetComponentType;

		public int componentIndex;

		public string fieldName;
	}
}