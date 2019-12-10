using System.Reflection;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
    [System.Serializable]
    public class PropertyReference {

        public Object target;

        public string fieldName;
        public PropertyInfo field { get; set; }
    }
}