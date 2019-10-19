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
        var randomized = UnityEngine.Random.Range(0, array.Length);
        if (randomized >= array.Length && array.Length > 0) {
            randomized = array.Length - 1;
        }
        if (randomized < 0 || array.Length <= randomized) {
            return default(T);
        }
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

    public static void AddOrSetRange<T, P>(this Dictionary<T, P> dic, Dictionary<T, P> other) {
        if (other == null) {
            throw new InvalidOperationException("`Other' param cant be null");
        }
        other.ForEach(kvp => {
            dic.AddOrSet(kvp.Key, kvp.Value);
        });
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
        return Enumerable.FirstOrDefault(enumerable);
    }

    public static T Last<T>(this IEnumerable<T> enumerable) {
        if (enumerable.Count() == 0) {
            return default(T);
        }
        return enumerable.ToList()[enumerable.Count() - 1];
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

    public static List<T> ToUniqueList<T>(this IEnumerable<T> enumerable) {
        var uniqueList = new List<T>();
        enumerable.ForEach(item => {
            if (!uniqueList.ContainsItem(item)) {
                uniqueList.Add(item);
            }
        });
        return uniqueList;
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

        sb.Append(string.Format("[{0}][", enumerable.Count()));
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

    public static IEnumerable<T> MaxElements<T>(this IEnumerable<T> enumerable, Func<T, float> selector) {
        var maxElements = new List<T>();
        enumerable.ForEach(e => {
            if (maxElements.Count == 0) {
                maxElements.Add(e);
            } else if (selector(maxElements.First()) == selector(e)) {
                maxElements.Add(e);
            } else if (selector(maxElements.First()) < selector(e)) {
                maxElements.Clear();
                maxElements.Add(e);
            }
        });
        return maxElements;
    }

    public static T MaxElement<T>(this IEnumerable<T> enumerable, Func<T, float> selector) {
        return MaxElements(enumerable, selector).First();
    }

    public static IEnumerable<T> MinElements<T>(this IEnumerable<T> enumerable, Func<T, float> selector) {
        var minElements = new List<T>();
        enumerable.ForEach(e => {
            if (minElements.Count == 0) {
                minElements.Add(e);
            } else if (selector(minElements.First()) == selector(e)) {
                minElements.Add(e);
            } else if (selector(minElements.First()) > selector(e)) {
                minElements.Clear();
                minElements.Add(e);
            }
        });
        return minElements;
    }

    public static T MinElement<T>(this IEnumerable<T> enumerable, Func<T, float> selector) {
        return MinElements(enumerable, selector).First();
    }

    public static void Sort<T>(this List<T> list, Func<T, float> comparer) {
        Sort(list, comparer, false);
    }
    public static void Sort<T>(this List<T> list, Func<T, float> comparer, bool invert) {
        list.Sort((e1, e2) => {
            if (comparer(e1) > comparer(e2)) {
                return !invert ? 1 : -1;
            } else if (comparer(e1) < comparer(e2)) {
                return !invert ? -1 : 1;
            } else {
                return 0;
            }
        });
    }

    public static void RemoveRange<T>(this List<T> list, IEnumerable<T> range) {
        range.ForEach(item => {
            if (list.Contains(item)) {
                list.Remove(item);
            }
        });
    }

    public static int IndexOf<T>(this IEnumerable<T> enumerable, T item) {
        return enumerable.ToList().IndexOf(item);
    }
}
