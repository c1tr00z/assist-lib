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
		return GetTypes().Where(t => t.Name == name).First();	
	}

    public static Type[] GetSubclassesOf(Type baseClass) {
        if (baseClass == null) {
            return GetTypes();
        }
        return GetTypes().Where(t => t.IsSubclassOf(baseClass)).ToArray();
    }

	public static FieldInfo GetPublicFieldInfo(this Type type, string fieldName) {
		return type.GetField(fieldName, System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
	}
}
