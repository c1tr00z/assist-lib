using UnityEngine;
using System.Collections;

public static class ObjectUtils {

    public static T Clone<T>(this T obj) where T : Object {
        return Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as T;
    }

    public static T Clone<T>(this T obj, Transform parent) where T : Component {
        var clone = obj.Clone<T>();
        clone.transform.parent = parent;

        clone.Reset();

        return clone;
    }

    public static void Reset<T>(this T obj) where T : Component {
        obj.Reset(null);
    }

    public static void Reset<T>(this T obj, Transform parent) where T : Component {
        if (parent != null) {
            obj.transform.SetParent(parent, false);
        }
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        var rot = Quaternion.identity;
        rot.eulerAngles = Vector3.zero;
        obj.transform.localRotation = rot;
    }
}
