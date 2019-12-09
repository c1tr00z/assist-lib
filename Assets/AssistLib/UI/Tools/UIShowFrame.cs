using System.Collections;
using c1tr00z.AssistLib.PropertyReferences;
using UnityEngine;

namespace c1tr00z.AssistLib.UI {
    public class UIShowFrame : MonoBehaviour {

        public UIFrameDBEntry frameDBEntry;

        public bool showOnStart = false;
        
        [ReferenceType(typeof(object))]
        [SerializeField]
        private PropertyReference[] _argsSources;

        private IEnumerator Start() {

            while (UI.instance == null) {
                yield return null;
            }

            if (showOnStart) {
                Show();
            }
        }

        public void Show() {
            if (frameDBEntry == null) {
                return;
            }

            var args = _argsSources == null
                ? new object[0]
                : _argsSources.SelectNotNull(s => s.Get<object>()).ToArray();
            frameDBEntry.Show();
        }
    }
}