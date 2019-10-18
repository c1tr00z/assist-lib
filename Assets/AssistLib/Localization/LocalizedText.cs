using UnityEngine;
using UnityEngine.UI;

namespace c1tr00z.AssistLib.Localization {
    [RequireComponent(typeof(Text))]
    public abstract class LocalizedText : MonoBehaviour {

        [SerializeField]
        protected string key = "Title";

        private void Awake() {
            Localize();
        }

        protected abstract string GetLocalizedText();

        public void Localize() {
            GetComponent<Text>().text = GetLocalizedText();
        }
    }
}