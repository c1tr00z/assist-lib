using c1tr00z.AssistLib.PropertyReferences;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.DataModels {
    public abstract class ValueReceiverBase : MonoBehaviour, IValueReceiver {

        private List<IDataModelBase> _models = null;
        
        public virtual bool isRecieverEnabled {
            get { return GetModels().All(m => m.isDataModelEnabled); }
        }

        protected virtual void Awake() {
			GetModels().ForEach(m => m.AddReceiver(this));
		}

        public IEnumerable<IDataModelBase> GetModels() {
            if (_models == null) {
                _models = new List<IDataModelBase>();
                var references = GetReferences();
                while (references.MoveNext()) {
                    var reference = references.Current;
                    var targetComponent = reference.GetTargetComponent();
                    var model = targetComponent as IDataModelBase;
                    if (model == null) {
                        continue;
                    }
                    if (!_models.Contains(model)) {
                        _models.Add(model);
                    }
                }
            }
                
            return _models;
        }

        public abstract IEnumerator<PropertyReference> GetReferences();

        public abstract void UpdateReceiver();
    }
}