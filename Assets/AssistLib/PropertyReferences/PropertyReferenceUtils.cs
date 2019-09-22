using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
	public static class PropertyReferenceUtils {

		public static T Get<T>(this PropertyReference propertyReference) {
            var componenetType = ReflectionUtils.GetTypeByName(propertyReference.targetComponentTypeName);
            var allComponents = propertyReference.target.GetComponents(componenetType).ToUniqueList();
            var component = allComponents[propertyReference.componentIndex];
            var type = component.GetType();
			var field = type.GetPublicPropertyInfo(propertyReference.fieldName);
			if (field == null) {
				return default(T);
			}
            var value = field.GetValue(component, null);
			if (value is T) {
				return (T)value;
			}
			return default(T);
		}
	}
}