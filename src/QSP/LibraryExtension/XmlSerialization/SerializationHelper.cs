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
    }
}
