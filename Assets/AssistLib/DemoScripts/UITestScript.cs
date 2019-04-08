using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UITestScript : MonoBehaviour {

    private int _clicksCount;

    private string _saveDataKey = "_save1";

    void Start() {
        Saves.instance.LoadData(_saveDataKey, b => {
            if (b) {
                _clicksCount = Saves.instance.LoadInt("buttonClicks");
                PrintClicks();
            } else {
                Debug.LogError("error");
            }
        });
        
    }

    public void ButtonClick() {
        _clicksCount++;
        Saves.instance.Save("buttonClicks", _clicksCount);

        Saves.instance.Save("v", new Vector3(155, 222, 340));

        Saves.instance.Save("d", new Dictionary<string, object>() { { "aa", 11 }, { "bb", "cc" }, {"dd", new Vector3(1, 2, 3)} });
        PrintClicks();
    }

    void PrintClicks() {
        Debug.Log("Total Clicks: " + _clicksCount);
        Debug.Log(Saves.instance.LoadObject("d"));
    }
}
