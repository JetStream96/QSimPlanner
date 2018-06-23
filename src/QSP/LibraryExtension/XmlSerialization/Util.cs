using System.IO;
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
    }
}
