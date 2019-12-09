using System.Collections.Generic;
using c1tr00z.AssistLib.PropertyReferences;

namespace c1tr00z.AssistLib.DataModels {
    public abstract class DataTranslator : DataModelBase, IValueReceiver {
        
        private List<IDataModelBase> _models = null;

        protected virtual void Awake() {
            GetModels().ForEach(m => m.AddReceiver(this));
        }
        
        #region IValueReceiver Implementation

        public bool isRecieverEnabled {
            get { return isDataModelEnabled && GetModels().All(m => m.isDataModelEnabled); }
        }
        
        public abstract void UpdateReceiver();

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

        #endregion

        #region Abstract Methods

        public abstract IEnumerator<PropertyReference> GetReferences();

        #endregion
    }
}