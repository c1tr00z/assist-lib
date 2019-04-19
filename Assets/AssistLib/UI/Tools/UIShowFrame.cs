using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UIShowFrame : MonoBehaviour {

        public UIFrameDBEntry frameDBEntry;

        public bool showOnStart = false;

        private IEnumerator Start() {

            while (UI.instance == null) {
                yield return null;
            }

            if (showOnStart) {
                Show();
            }
        }

        public void Show() {
            if (frameDBEntry != null) {
                UI.instance.Show(frameDBEntry);
            }
        }
    }
}