using NUnit.Framework;
using QSP.LibraryExtension.XmlSerialization;
using System.Xml.Linq;

namespace UnitTest.LibraryExtension.XmlSerialization
{
    [TestFixture]
    public class SerializationHelperTest
    {
        [Test]
        public void SerializeIntTest()
        {
            var e = 103.Serialize("a");
            Assert.AreEqual("103", e.Element("a").Value);
        }

        [Test]
        public void GetStringTest()
        {
            var e = XElement.Parse("<e>hello<e>");
            Assert.AreEqual("hello", e.GetString("e"));
        }

        [Test]
        public void GetIntTest()
        {
            var e = XElement.Parse("<e>-84<e>");
            Assert.AreEqual(-84, e.GetInt("e"));
        }
    }
}
