using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace c1tr00z.AssistLib.Localization {
    public static class StringUtils {

        public static string DecodeEncodedNonAscii(this string value) {

            return Regex.Replace(value, @"\\u(?<Value>[a-zA-Z0-9]{4})", m => {
                return ((char)Int16.Parse(m.Groups["Value"].Value, NumberStyles.AllowHexSpecifier)).ToString();
            });
        }
    }
}
