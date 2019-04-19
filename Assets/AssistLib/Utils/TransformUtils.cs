using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TransformUtils {

    public static IEnumerable<Transform> GetChildren(this Transform transform) {
        var list = new List<Transform>();
        for (var i = 0; i < transform.childCount; i++) {
            list.Add(transform.GetChild(i));
        }
        return list;
    }

    public static void SetChildrenSiblingIndex(this Transform transform, System.Func<Transform, int> siblingIndexSelector) {
        GetChildren(transform).ForEach(c => c.SetSiblingIndex(siblingIndexSelector(c)));
    }

    public static Vector2 GetUIScreenPosition(this Transform transform) {
        var scaler = transform.GetComponentInParent<CanvasScaler>();
        var camera = Camera.allCameras.Where(c => (c.cullingMask & (1 << 5)) == 0).First();

        var cameraPosition = camera.WorldToScreenPoint(transform.position).ToVector2();
        var scale = scaler.transform.localScale.ToVector2();

        return new Vector2(cameraPosition.x / scale.x, cameraPosition.y / scale.y);
    }
}
