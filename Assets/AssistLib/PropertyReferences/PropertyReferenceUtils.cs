using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace c1tr00z.AssistLib.PropertyReferences {
    public static class PropertyReferenceUtils {
        
        private static Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();

        public static T Get<T>(this PropertyReference propertyReference) {
            if (propertyReference.field == null) {
                var key = propertyReference.GetPropertyKey();
                propertyReference.component = propertyReference.GetTargetComponent();
                if (!_properties.ContainsKey(key)) {
                    var type = propertyReference.component.GetType();
                    _properties.AddOrSet(key, type.GetPublicPropertyInfo(propertyReference.fieldName));
                }

                propertyReference.field = _properties[key];

            }
            if (propertyReference.field == null) {
                return default(T);
            }

            var value = propertyReference.GetValue();
            if (typeof(T) == typeof(string)) {
                if (value == null) {
                    return default(T);
                }
                return (T)(object)propertyReference.GetValue().ToString();
            }
            if (value is T) {
                return (T)value;
            }
            return default(T);
        }

        private static string GetPropertyKey(this PropertyReference propertyReference) {
            return $"{propertyReference.targetComponentTypeName}.{propertyReference.fieldName}";
        }

        public static object GetValue(this PropertyReference propertyReference) {
            return propertyReference.field.GetValue(propertyReference.component, null);
        }

        public static Component GetTargetComponent(this PropertyReference propertyReference) {
            var componenetType = ReflectionUtils.GetTypeByName(propertyReference.targetComponentTypeName);
            var allComponents = propertyReference.target.GetComponents(componenetType).ToUniqueList();
            return allComponents[propertyReference.componentIndex];
        }
    }
}