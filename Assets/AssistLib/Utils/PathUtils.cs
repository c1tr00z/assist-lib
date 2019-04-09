using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PathUtils {
    
    public static string Combine(params string[] paths) {
        if (paths == null || paths.Length == 0) {
            return "";
        } else if (paths.Length == 1) {
            return paths[0];
        } else if (paths.Length == 2) {
            return Path.Combine(paths[0], paths[1]);
        } else {
            var path = paths[0];
            for (var i = 1; i < paths.Length; i++) {
                path = Path.Combine(path, paths[i]);
            }
            return path;
        }
    }
}
