using System.Collections.Generic;
using c1tr00z.AssistLib.PropertyReferences;

namespace c1tr00z.AssistLib.DataModels {
    public abstract class DataTranslator : DataModelBase, IValueReceiver {

        #region IValueReceiver Implementation

        public abstract void UpdateReceiver();

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

        #endregion

        #region Abstract Methods

        public abstract IEnumerator<PropertyReference> GetReferences();

        #endregion
    }
}