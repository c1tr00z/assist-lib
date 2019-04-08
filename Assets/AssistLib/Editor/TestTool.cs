using UnityEngine;
using System.Collections;

[EditorToolName("Test Tool")]
public class TestTool : EditorTool {

    protected override void DrawInterface() {
        Label("TEST");
    }
}
