using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class IDictionary
    {
        public static IReadOnlyDictionary<K,V> Update<K, V>(this IReadOnlyDictionary<K, V> old,
            IEnumerable<KeyValuePair<K, V>> newEntries)
        {
            var d = old.ToDictionary();
            newEntries.ForEach(kv => d[kv.Key] = kv.Value);
            return d;
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K, V>> x)
        {
            return x.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
