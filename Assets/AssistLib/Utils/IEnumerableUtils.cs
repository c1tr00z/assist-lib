using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class IEnumerableUtils {
    
    public static T RandomItem<T>(this IEnumerable<T> items) {

        T[] array;
        if (items is T[]) {
            array = items as T[];
        } else {
            var list = new List<T>();
            list.AddRange(items);
            array = list.ToArray();
        }
        var randomized = UnityEngine.Random.Range(0, array.Length - 1);
        return array[randomized];
    }

    public static void AddOrSet<T, P>(this Dictionary<T, P> dic, T key, P value) {
        if (key == null) {
            Debug.LogError(string.Format("Key cannot be null (value: {0})", value));
            return;
        }
        if (dic.ContainsKey(key)) {
            dic[key] = value;
        } else {
            dic.Add(key, value);
        }
    }

    public static bool ContainsItem<T>(this IEnumerable<T> enumerable, T item) {
        foreach (T i in enumerable) {
            if (i.Equals(item)) {
                return true;
            }
        }
        return false;
    }

    public static void ForEach<T>(this IEnumerable<T> enumerable, System.Action<T> action) {
        enumerable.ToList().ForEach(action);
    }

    public static T First<T>(this IEnumerable<T> enumerable) {
        foreach (T t in enumerable) {
            if (!t.Equals(default(T))) {
                return t;
            }
        }
        return default(T);
    }

    public static IEnumerable<T> Where<T>(this IEnumerable<T> enumerable, System.Func<T, bool> selector) {
        return Enumerable.Where(enumerable, selector);
    }

    public static IEnumerable<T2> Select<T1, T2>(this IEnumerable<T1> enumerable, System.Func<T1, T2> processor) {
        return Enumerable.Select(enumerable, processor);
    }

    public static IEnumerable<T2> SelectNotNull<T1, T2>(this IEnumerable<T1> enumerable, System.Func<T1, T2> processor) {
        return Select(enumerable, processor).Where(item => item != null);
    }

    public static IEnumerable<T> SelectNotNull<T>(this IEnumerable<T> enumerable) {
        return enumerable.Where(item => item != null);
    }

    public static Dictionary<T2, T3> ToDictionary<T1, T2, T3>(this IEnumerable<T1> enumerable, System.Func<T1, T2> keySelector, System.Func<T1, T3> valueSelector) {
        var dic = new Dictionary<T2, T3>();
        enumerable.ForEach(i => {
            dic.Add(keySelector(i), valueSelector(i));
        });
        return dic;
    }

    public static List<T> ToList<T>(this IEnumerable<T> enumerable) {
        if (enumerable is List<T>) {
            return enumerable as List<T>;
        }
        return new List<T>(enumerable);
    }

    public static T[] ToArray<T>(this IEnumerable<T> enumerable) {

        if (enumerable == null) {
            return null;
        }

        var list = new List<T>();
        list.AddRange(enumerable);
        return list.ToArray();
    }

    public static string ToPlainString(this Dictionary<object, object> dic) {
        var sb = new System.Text.StringBuilder();

        System.Func<object, string> processor = (val) => {
            if (val is Dictionary<object, object>) {
                return ((Dictionary<object, object>)val).ToPlainString();
            } else if (val is IEnumerable<object>) {
                return ((IEnumerable<object>)val).ToPlainString();
            }
            return val.ToString();
        };

        sb.Append("[");
        foreach (KeyValuePair<object, object> kvp in dic) {
            sb.Append(string.Format("{0}:{1}", processor(kvp.Key), processor(kvp.Value)));
        }
        sb.Append("]");

        return sb.ToString();
    }

    public static string ToPlainString<T>(this IEnumerable<T> enumerable) {
        var sb = new System.Text.StringBuilder();

        sb.Append("[");
        foreach (T item in enumerable) {
            sb.Append(item + ", ");
        }
        sb.Append("]");

        return sb.ToString();
    }

    public static int Count<T>(this IEnumerable<T> enumerable) {
        return enumerable.ToArray().Length;
    }

    public static int Min(this IEnumerable<int> enumerable) {
        var min = 0;
        foreach (var item in enumerable) {
            min = item < min ? item : min;
        }
        return min;
    }

    public static bool All<T>(this IEnumerable<T> enumerable, System.Func<T, bool> predicate) {
        return System.Linq.Enumerable.All(enumerable, predicate);
    }

    public static IEnumerable<T> SubArray<T>(this IEnumerable<T> enumerable, int index) {
        var length = enumerable.Count() - index;
        return SubArray(enumerable, index, length);
    }

    public static IEnumerable<T> SubArray<T>(this IEnumerable<T> enumerable, int index, int length) {
        T[] result = new T[length];
        Array.Copy(enumerable.ToArray(), index, result, 0, length);
        return result;
    }
}
