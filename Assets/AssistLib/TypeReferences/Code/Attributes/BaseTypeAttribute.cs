using UnityEngine;

namespace c1tr00z.AssistLib.TypeReferences {

    public class BaseTypeAttribute : PropertyAttribute {
        public System.Type type { get; private set; }
        
        public bool includeBaseClass { get; private set; }

        public BaseTypeAttribute (System.Type type, bool includeBaseClass = false) {
            this.type = type;
            this.includeBaseClass = includeBaseClass;
        }
        
        public override bool Match (object obj) {
            var other = obj as BaseTypeAttribute;
            if (other == null) {
                return false;
            }

            return other.type == this.type;
        }
    }
}
