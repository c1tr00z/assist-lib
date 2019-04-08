using UnityEngine;
using System.Collections;

public class UIFrameSceneButton : MonoBehaviour {

	[SerializeField] private SceneItem _scene;
    [SerializeField] private UIFrameItem _frame;

    public void Load() {
        if (_scene != null) {
            Scenes.instance.LoadScene(_scene);
        }
        if (_frame != null) {
            UI.instance.Show(_frame);
        }
    }
}
