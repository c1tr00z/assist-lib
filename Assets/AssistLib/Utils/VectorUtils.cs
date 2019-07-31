using System.Globalization;
using UnityEngine;

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

    public static Vector3 RandomV3(float minValue, float maxValue) {
        return new Vector3(Random.Range(minValue, maxValue), Random.Range(minValue, maxValue), Random.Range(minValue, maxValue));
    }

    public static Vector3 RandomV3(int minValue, int maxValue) {
        return new Vector3(Random.Range(minValue, maxValue), Random.Range(minValue, maxValue), Random.Range(minValue, maxValue));
    }

    public static Vector3 RandomV3(Vector3 minValue, Vector3 maxValue) {
        return new Vector3(Random.Range(minValue.x, maxValue.x), Random.Range(minValue.y, maxValue.y), Random.Range(minValue.z, maxValue.z));
    }

    /* 
     * found https://answers.unity.com/questions/1134997/string-to-vector3.html
     * then modified
    */

    private static string[] SplitVectorString(string vectorString) {
        if (string.IsNullOrEmpty(vectorString)) {
            return new string[0];
        }
        // Remove the parentheses
        if (vectorString.StartsWith("(") && vectorString.EndsWith(")")) {
            vectorString = vectorString.Substring(1, vectorString.Length - 2);
        }
        
        return vectorString.Split(',');
    }

    public static bool TryParse(string vectorString, out Vector4 result) {
        result = Vector4.zero;
        var sArray = SplitVectorString(vectorString);

        if (sArray.Length < 2) {
            return false;
        }

        var resultArray = new float[4];

        for (var i = 0; i < 4; i++) {
            if (i >= sArray.Length) {
                break;
            }
            float floatValue;
            if (float.TryParse(sArray[i], NumberStyles.Float, NumberFormatInfo.InvariantInfo, out floatValue)) {
                resultArray[i] = floatValue;
            } else {
                return false;
            }
        }

        // store as a Vector4
        result = new Vector4(resultArray[0], resultArray[1], resultArray[2], resultArray[3]);

        return true;
    }

    public static bool TryParse(string vectorString, out Vector3 result) {
        result = Vector3.zero;
        Vector4 vector4;
        if (TryParse(vectorString, out vector4)) {
            result = vector4.ToVector3();
            return true;
        }
        return false;
    }

    public static bool TryParse(string vectorString, out Vector2 result) {
        result = Vector2.zero;
        Vector4 vector4;
        if (TryParse(vectorString, out vector4)) {
            result = vector4.ToVector2();
            return true;
        }
        return false;
    }

    public static string ToString(this Vector4 vector, string format, CultureInfo cultureInfo) {
        return string.Format("\"({0}, {1}, {2}, {3})\"",
            vector.x.ToString(format, cultureInfo),
            vector.y.ToString(format, cultureInfo),
            vector.z.ToString(format, cultureInfo),
            vector.w.ToString(format, cultureInfo));
    }

    public static string ToString(this Vector3 vector, string format, CultureInfo cultureInfo) {
        return string.Format("\"({0}, {1}, {2})\"",
            vector.x.ToString(format, cultureInfo),
            vector.y.ToString(format, cultureInfo),
            vector.z.ToString(format, cultureInfo));
    }

    public static string ToString(this Vector2 vector, string format, CultureInfo cultureInfo) {
        return string.Format("\"({0}, {1})\"",
            vector.x.ToString(format, cultureInfo),
            vector.y.ToString(format, cultureInfo));
    }

    public static string ToInvariantCultureString(this Vector2 vector) {
        return vector.ToString("G", CultureInfo.InvariantCulture);
    }

    public static string ToInvariantCultureString(this Vector3 vector) {
        return vector.ToString("G", CultureInfo.InvariantCulture);
    }

    public static string ToInvariantCultureString(this Vector4 vector) {
        return vector.ToString("G", CultureInfo.InvariantCulture);
    }
}
