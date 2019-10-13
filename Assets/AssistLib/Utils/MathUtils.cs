public static class MathUtils {
    
    public static bool Even(this int x) {
        return (x & 1) == 0;
    }

    public static void DoTimes(this int times, System.Action<int> action) {
        for (int i = 0; i < times; i++) {
            action.SafeInvoke(i);
        }
    }
}
