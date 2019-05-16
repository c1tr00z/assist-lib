using UnityEngine;
using System.Collections;

public static class ObjectUtils {

    public static T Clone<T>(this T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object {
        return Object.Instantiate(original, position, rotation, parent) as T;
    }

    public static T Clone<T>(this T obj) where T : Object {
        return Clone(obj, null);
    }

    public static T Clone<T>(this T obj, Transform parent) where T : Object {
        return Clone(obj, Vector3.zero, Quaternion.identity, parent);
    }

    public static T Clone<T>(this T obj, Vector3 position, Transform parent) where T : Object {
        return Clone(obj, position, Quaternion.identity, parent);
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
