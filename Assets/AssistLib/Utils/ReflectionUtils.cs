using System;
using System.Collections.Generic;

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

    public static Type[] GetSubclassesOf(Type baseClass) {
        if (baseClass == null) {
            return GetTypes();
        }
        return GetTypes().Where(t => t.IsSubclassOf(baseClass)).ToArray();
    }
}
