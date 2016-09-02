using System.IO;
using System.Xml.Serialization;

namespace QSP.LibraryExtension
{
    public class XmlSerialization
    {
        public static T Deserialize<T>(string data)
        {
            var deserializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(data))
            {
                return (T)deserializer.Deserialize(reader);
            }
        }

        public static string Serialize<T>(object o)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, o);
                return writer.ToString();
            }
        }
    }
}
