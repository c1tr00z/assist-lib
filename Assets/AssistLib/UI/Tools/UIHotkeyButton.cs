using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(Button))]
    public class UIHotkeyButton : MonoBehaviour {

        public KeyCode key = KeyCode.Escape;

        public void Update() {
            if (Input.GetKeyUp(key)) {
                CheckAndInvoke();
            }            
        }

        private void CheckAndInvoke() {
            if (GetComponentInParent<UIFrame>().isTopFrame) {
                GetComponent<Button>().onClick.Invoke();
            }
        }
    }
}