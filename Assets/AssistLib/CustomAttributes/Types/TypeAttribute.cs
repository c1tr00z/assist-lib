using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class TypeAttribute : PropertyAttribute {

    public Type baseType { get; set; } 

    public TypeAttribute() {
    }

    public TypeAttribute(Type baseType) {
        this.baseType = baseType;
    }
}
    