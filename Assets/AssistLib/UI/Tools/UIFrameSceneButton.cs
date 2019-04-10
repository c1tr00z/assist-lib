using c1tr00z.AssistLib.UI;
using UnityEngine;

namespace c1tr00z.AssistLib {
    public class UIFrameSceneButton : MonoBehaviour {

        [SerializeField] private SceneItem _scene;
        [SerializeField] private UIFrameDBEntry _frame;

        public void Load() {
            if (_scene != null) {
                Scenes.instance.LoadScene(_scene);
            }
            if (_frame != null) {
                UI.UI.instance.Show(_frame);
            }
        }
    }
}
