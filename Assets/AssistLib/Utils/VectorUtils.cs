using UnityEngine;
using System.Collections;
//using LitJson;

public static class VectorUtils {


    public static Vector2 ToVector2(this Vector4 v) {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 ToVector2(this Vector3 v) {
        return new Vector2(v.x, v.y);
    }

    public static Vector3 ToVector3(this Vector4 v) {
        return new Vector3(v.x, v.y, v.z);
    }

    public static Vector3 ToVector3(this Vector2 v) {
        return new Vector3(v.x, v.y, 0);
    }

    public static Vector4 ToVector4(this Vector3 v) {
        return new Vector4(v.x, v.y, v.z, 0);
    }

    public static Vector4 ToVector4(this Vector2 v) {
        return new Vector4(v.x, v.y, 0, 0);
    }
}
