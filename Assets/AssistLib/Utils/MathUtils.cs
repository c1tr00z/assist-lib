using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils {
    
    public static bool Even(this int x) {
        return (x & 1) == 0;
    }
}
