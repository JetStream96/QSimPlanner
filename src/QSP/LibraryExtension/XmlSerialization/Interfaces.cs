using System.Xml.Linq;

namespace QSP.LibraryExtension.XmlSerialization
{
    public interface IXSerializer<T>
    {
        XElement Serialize(T item, string name);
        T Deserialize(XElement elem);
    }
}
