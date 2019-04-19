using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UILoadScene : MonoBehaviour {

        public SceneItem sceneDBEntry;

        public void Load() {
            UI.instance.CloseAllFrames();
            Scenes.instance.LoadScene(sceneDBEntry);
        }
    }
}