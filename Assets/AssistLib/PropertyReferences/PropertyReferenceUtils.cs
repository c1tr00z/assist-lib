using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
	public static class PropertyReferenceUtils {
		public static void Refresh (this PropertyReference propertyReference) {
#if UNITY_EDITOR

			if (propertyReference.target == null) {
				throw new UnityException("Target is not assigned");
			}

			var parsedString = propertyReference.referenceString.Split('/');
			if (parsedString.Length < 3) {
				throw new UnityException("Reference string is not correct: " + propertyReference.referenceString);
			}
			var componentTypeName = parsedString[0];
			var componentIndex = parsedString[1];
			var fieldName = parsedString[2];

			var componentType = ReflectionUtils.GetTypeByName(componentTypeName);
			if (componentType == null) {
				throw new UnityException("Component type is not exist: " + componentType);
			}
			
			int index;
			if (!int.TryParse(componentIndex, out index)) {
				throw new UnityException("Component index is not correct: " + parsedString);
			}

			var fieldInfo = componentType.GetField(fieldName, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
			if (fieldInfo == null) {
				throw new UnityException("Component field is not correct: " + fieldName);
			}

			propertyReference.targetComponentType = componentType;
			propertyReference.componentIndex = index;
			propertyReference.fieldName = fieldName;
#endif
		}

		public static T Get<T>(this PropertyReference propertyReference) {
			var component = propertyReference.target.GetComponents(propertyReference.targetComponentType)[propertyReference.componentIndex];
			var type = propertyReference.targetComponentType;
			var field = type.GetPublicFieldInfo(propertyReference.fieldName);
			if (field == null) {
				return default(T);
			}
			var value = field.GetValue(component);
			if (value is T) {
				return (T)value;
			}
			return default(T);
		}
	}
}