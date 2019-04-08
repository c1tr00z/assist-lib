using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;

public static class JSONUtuls {
    public static string Serialize<T>(IEnumerable<T> e) {
        return Json.Serialize(e);
    }
}
