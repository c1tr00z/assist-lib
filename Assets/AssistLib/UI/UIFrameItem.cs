using UnityEngine;
using System.Collections;

public class UIFrameItem : DBEntry {

    public UIFrame LoadFrame() {
        return this.LoadPrefab<UIFrame>().Clone();
    }

    public void Show() {
        UI.instance.Show(this);
    }
}
