using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace c1tr00z.AssistLib.UI {
    [RequireComponent(typeof(EventTrigger))]
    public class UIPressedButton : MonoBehaviour {

        public UnityEvent OnPressedEvent;

        private bool _pressed;
        
        public void OnPressed() {
            _pressed = true;
        }

        public void OnReleased() {
            _pressed = false;
        }

        private void Update() {
            if (_pressed) {
                OnPressedEvent.Invoke();
            }
        }
    }
}