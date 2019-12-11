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
                if (!_properties.ContainsKey(key)) {
                    var type = propertyReference.target.GetType();
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

        public static List<T> GetList<T>(this PropertyReference propertyReference) {
            var iList = (IList)propertyReference.Get<object>();
            if (iList == null) {
                return null;
            }
            var list = new List<T>();
            var listEnum = iList.GetEnumerator();
            while (listEnum.MoveNext()) {
                var newItem = (T)listEnum.Current;
                if (newItem != null) {
                    list.Add(newItem);
                }
            }

            return list;
        }

        private static string GetPropertyKey(this PropertyReference propertyReference) {
            return $"{propertyReference.target.GetType().FullName}.{propertyReference.fieldName}";
        }

        public static object GetValue(this PropertyReference propertyReference) {
            return propertyReference.field.GetValue(propertyReference.target, null);
        }
    }
}