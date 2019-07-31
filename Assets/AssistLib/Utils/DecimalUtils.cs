using System.Globalization;

public static class DecimalUtils {
    public static string ToInvariantCultureString(this float floatValue) {
        return floatValue.ToString("G", CultureInfo.InvariantCulture);
    }
}
