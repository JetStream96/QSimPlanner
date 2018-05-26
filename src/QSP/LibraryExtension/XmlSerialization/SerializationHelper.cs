using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace QSP.LibraryExtension.XmlSerialization
{
    public static class SerializationHelper
    {
        public static XElement Serialize(this string value, string key)
        {
            // The XElement escapes the chars automatically.
            return new XElement(key, value);
        }

        public static XElement Serialize(this int value, string key)
        {
            return new XElement(key, value.ToString());
        }

        public static XElement Serialize(this double value, string key)
        {
            return new XElement(key, value.ToString());
        }

        public static XElement Serialize(this bool value, string key)
        {
            return new XElement(key, value.ToString());
        }

        /// <summary>
        /// Serialize ['a', 'b', 'c'] to:
        ///     <key>
        ///         <e>a</e>
        ///         <e>b</e>
        ///         <e>c</e>
        ///     </key>
        /// </summary>
        public static XElement Serialize(this IEnumerable<string> val, string key)
        {
            return new XElement(key, val.Select(x => new XElement("e", x)));
        }

        /// <summary>
        /// Serialize {'a':'1', 'b':'2'} to:
        ///     <key>
        ///         <a>1</a>
        ///         <b>2</b>
        ///     </key>
        /// </summary>
        public static XElement Serialize(this IReadOnlyDictionary<string, string> dict, 
            string key)
        {
            return new XElement(key, dict.Select(kv => new XElement(kv.Key, kv.Value)));
        }

        public static string GetString(this XElement elem, string key)
        {
            return elem.Element(key).Value;
        }

        public static bool GetBool(this XElement elem, string key)
        {
            return bool.Parse(elem.GetString(key));
        }

        public static int GetInt(this XElement elem, string key)
        {
            return int.Parse(elem.GetString(key));
        }

        public static double GetDouble(this XElement elem, string key)
        {
            return double.Parse(elem.GetString(key));
        }

        public static string GetAttributeString(this XElement e, string key)
        {
            return e.Attribute(key).Value;
        }

        public static double GetAttributeDouble(this XElement e, string key)
        {
            return double.Parse(e.GetAttributeString(key));
        }

        public static IEnumerable<string> GetArray(this XElement e, string key)
        {
            return e.Element(key).Elements("e").Select(x => x.Value);
        }

        public static Dictionary<string, string> GetDict(this XElement e, string key)
        {
            return e.Element(key).Elements().ToDictionary(x => x.Name.LocalName, x => x.Value);
        }
    }
}
