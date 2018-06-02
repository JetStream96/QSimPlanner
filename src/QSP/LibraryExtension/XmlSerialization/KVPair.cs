using System.Xml.Serialization;

namespace QSP.LibraryExtension.XmlSerialization
{
    /// <summary>
    /// Helper class for serializing dictionary.
    /// </summary>
    public class KVPair<K, V>
    {
        [XmlElement("Key")]
        public K Key { get; set; }

        [XmlElement("Value")]
        public V Value { get; set; }
    }
}
