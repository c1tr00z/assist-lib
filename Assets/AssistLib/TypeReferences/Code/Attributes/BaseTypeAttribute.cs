using UnityEngine;

namespace c1tr00z.AssistLib.TypeReferences {

    public class BaseTypeAttribute : PropertyAttribute {
        public System.Type type { get; private set; }

        public BaseTypeAttribute (System.Type type) {
            this.type = type;
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
