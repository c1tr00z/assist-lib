using System.Collections;
using System.Collections.Generic;
using c1tr00z.AssistLib.TypeReferences;
using UnityEngine;

public class TypeReferencesExample : MonoBehaviour {
    [BaseType(typeof(MonoBehaviour))]
    public TypeReference[] mahType;
}
