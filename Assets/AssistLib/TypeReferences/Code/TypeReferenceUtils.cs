using System;

namespace c1tr00z.AssistLib.TypeReferences {
    public static class TypeReferenceUtils {
        public static Type GetTypeFromReference(this TypeReference typeReference) {
            return ReflectionUtils.GetTypeByName(typeReference.typeFullName);
        }
    }
}