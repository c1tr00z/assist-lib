using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceTypeAttribute : System.Attribute {

    public System.Type type { get; private set; }

    public ReferenceTypeAttribute(System.Type type) {
        this.type = type;
    }

    public override bool Match(object obj) {
        var other = obj as ReferenceTypeAttribute;
        if (other == null) {
            return false;
        }

        return other.type == this.type;
    }
}
