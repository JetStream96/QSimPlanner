using NUnit.Framework;
using System.Xml.Linq;
using static QSP.LibraryExtension.Types;
using QSP.LibraryExtension.XmlSerialization;
using System.Linq;

namespace UnitTest.LibraryExtension.XmlSerialization
{
    [TestFixture]
    public class SerializationHelperTest
    {
        [Test]
        public void SerializeArrayTest()
        {
            var a = Arr("a", "b", "c");

            var e = a.Serialize("key0");

            Assert.AreEqual("key0", e.Name.LocalName);

            var elements = e.Elements().Select(x => x.Value).ToList();
            Assert.AreEqual(3, elements.Count);
            Assert.AreEqual("a", elements[0]);
            Assert.AreEqual("b", elements[1]);
            Assert.AreEqual("c", elements[2]);
        }

        [Test]
        public void SerializeDictTest()
        {
            var a = Dict(("a", "1"), ("b", "2"), ("c", "3"));

            var e = a.Serialize("key0");

            Assert.AreEqual("key0", e.Name.LocalName);

            var elements = e.Elements().Select(x => (x.Name.LocalName, x.Value)).ToList();
            Assert.AreEqual(3, elements.Count);
            Assert.AreEqual(("a", "1"), elements[0]);
            Assert.AreEqual(("b", "2"), elements[1]);
            Assert.AreEqual(("c", "3"), elements[2]);
        }

        [Test]
        public void DeserializeArrayTest()
        {
            var x = new XElement("doc",
                        new XElement("key",
                                     Arr(new XElement("e", "0"), new XElement("e", "1"))));

            var arr = x.GetArray("key").ToArray();
            Assert.AreEqual(2, arr.Length);
            Assert.AreEqual("0", arr[0]);
            Assert.AreEqual("1", arr[1]);
        }

        [Test]
        public void DeserializeDictTest()
        {
            var x = new XElement("doc", 
                        new XElement("key", 
                                     Arr(new XElement("a", "0"), new XElement("b", "1"))));
            var dict = x.GetDict("key");
            Assert.AreEqual(2, dict.Count);
            Assert.AreEqual("0", dict["a"]);
            Assert.AreEqual("1", dict["b"]);
        }
    }
}
