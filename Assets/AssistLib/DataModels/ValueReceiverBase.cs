using c1tr00z.AssistLib.PropertyReferences;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public abstract class ValueReceiverBase : MonoBehaviour, IValueReceiver {

		protected virtual void Awake() {
			GetModels().ForEach(m => m.AddReceiver(this));
		}

        public IEnumerable<IDataModelBase> GetModels() {
            var modelsList = new List<IDataModelBase>();
            var references = GetReferences();
            while (references.MoveNext()) {
                var reference = references.Current;
                var targetComponent = reference.GetTargetComponent();
                if (targetComponent is IDataModelBase) {
                    modelsList.Add(targetComponent as IDataModelBase);
                }
            }
            return modelsList;
        }

        public abstract IEnumerator<PropertyReference> GetReferences();

        public abstract void UpdateReceiver();
    }
}