using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ComponentUtils {
    
    public static bool TryGetComponent<T>(this Component comp, out T targetComponent) {
        targetComponent = comp.GetComponent<T>();
        if (targetComponent != null) {
            return true;
        }
        return false;
    }

}
