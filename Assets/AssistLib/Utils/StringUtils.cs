using System;
using System.Collections.Generic;
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
        
       public static string GenerateWord(int lenght, bool fromCapital) {
            var vovels = "AEIOUY".ToCharArray();
            var consonants = "BCDFGHJKLMNPQRSTVWXZ".ToCharArray();

            if (lenght == 0) {
                throw new Exception("Lenght cannot be 0");
            }

            if (lenght == 1) {
                var letter = vovels.RandomItem().ToString();
                return fromCapital ? letter.ToUpper() : letter.ToLower();
            }

            var fromVowels = UnityEngine.Random.Range(0, 2) == 0;
            var str = "";
            var prevIsFromVowels = !fromVowels;
            int sameTypeCounter = 0;
            int maxAllowedSameType = !fromVowels && lenght < 3 ? 1 : 2;
            for (var i = 0; i < lenght; i++) {
                str += fromVowels ? vovels.RandomItem() : consonants.RandomItem();
                fromVowels = UnityEngine.Random.Range(0, 2) == 0;
                if (prevIsFromVowels == fromVowels) {
                    sameTypeCounter++;
                } else {
                    sameTypeCounter = 1;
                }

                if (sameTypeCounter >= maxAllowedSameType) {
                    fromVowels = !fromVowels;
                    sameTypeCounter = 0;
                }
                
                prevIsFromVowels = fromVowels;
            }

            str = str.ToLower();

            var head = str.First().ToString();
            var tail = str.Substring(1, str.Length - 1);
            return $"{(fromCapital ? head.ToUpper() : head)}{tail}";
        }
        public static string GenerateSentence(int wordsCount, int minWordLenght, int maxWordLenght, bool everyWordIsCapital = false) {
            var wordsList = new List<string>();
            wordsCount.DoTimes(i => wordsList.Add(GenerateWord(UnityEngine.Random.Range(minWordLenght, maxWordLenght + 1)
            , i == 0 || everyWordIsCapital ? true : false)));
            return string.Join(" ", wordsList).Trim();
        }
    }
}