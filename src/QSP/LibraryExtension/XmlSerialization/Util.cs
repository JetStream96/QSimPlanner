using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QSP.LibraryExtension.XmlSerialization
{
    public static class Util
    {
        public static string SerializeXml<T>(T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            var sb = new StringBuilder();
            serializer.Serialize(new StringWriter(sb), obj);
            return sb.ToString();
        }

        public static T DeserializeXml<T>(string xmlContent)
        {
            using (var reader = new StringReader(xmlContent))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }

        public static KVPair<K, V>[] ToKVPairs<K, V>(this IReadOnlyDictionary<K, V> d)
        {
            return d.Select(kv => new KVPair<K, V>() { Key = kv.Key, Value = kv.Value })
                    .ToArray();
        }

        public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KVPair<K, V>> x)
        {
            return x.ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
