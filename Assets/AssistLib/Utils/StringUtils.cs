using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Random = System.Random;

namespace c1tr00z.AssistLib.Localization {
    public static class StringUtils {
        private static Random random = new Random();

        public static string DecodeEncodedNonAscii(this string value) {
            return Regex.Replace(value, @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char) short.Parse(m.Groups["Value"].Value, NumberStyles.AllowHexSpecifier)).ToString();
                });
        }

        public static string RandomString(int length, bool useSpaces = false, bool useNumbers = false) {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (useSpaces) {
                chars += "  ";
            }

            if (useNumbers) {
                chars += "0123456789";
            }
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}