using System;
using System.Collections.Generic;
using System.Reflection;

public static class ReflectionUtils {

    private static Type[] _types;

    public static Type[] GetTypes() {
        if (_types == null || _types.Length == 0) {
            var typesList = new List<Type>();
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            assemblies.ForEach(assembly => {
                typesList.AddRange(assembly.GetTypes());
            });
            _types = typesList.ToArray();
        }
        return _types;
    }

	public static Type GetTypeByName(string name) {
		return GetTypes().Where(t => t.FullName == name).First();	
	}

    public static Type[] GetSubclassesOf(Type baseClass) {
        if (baseClass == null) {
            return GetTypes();
        }
        return GetTypes().Where(t => t.IsSubclassOf(baseClass)).ToArray();
    }

    public static IEnumerable<PropertyInfo> GetPublicProperties(this Type type) {
        return type.GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
    }

	public static PropertyInfo GetPublicPropertyInfo(this Type type, string fieldName) {
		return type.GetProperty(fieldName, BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public);
	}

	public static string GetPropertyNameByType(this PropertyInfo propertyInfo) {
		return string.Format("{0}/{1}", propertyInfo.PropertyType.Name, propertyInfo.Name);
	}
}
