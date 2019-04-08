using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

public static class TransformUtils {
    
    public static IEnumerable<Transform> GetChildren(this Transform transform) {
        var list = new List<Transform>();
        for (var i = 0; i < transform.childCount; i++) {
            list.Add(transform.GetChild(i));
        }
        return list;
     }

}
