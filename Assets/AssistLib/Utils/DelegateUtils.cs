

public static class DelegateUtils {
    
    public static void SafeInvoke(this System.Action action) {
        if (action != null) {
            action.Invoke();
        }
    }

    public static void SafeInvoke<T>(this System.Action<T> action, T param1) {
        if (action != null) {
            action.Invoke(param1);
        }
    }

    public static void SafeInvoke<T1, T2>(this System.Action<T1, T2> action, T1 param1, T2 param2) {
        if (action != null) {
            action.Invoke(param1, param2);
        }
    }
}
