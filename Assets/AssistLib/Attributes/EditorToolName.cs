using UnityEngine;
using System.Collections;

public class EditorToolName : System.Attribute {

    public string toolName;

    public EditorToolName(string toolName) {
        this.toolName = toolName;
    }

    public override bool Match(object obj) {
        var other = obj as EditorToolName;
        if (other == null) {
            return false;
        }

        return other.toolName == this.toolName;
    }
}
