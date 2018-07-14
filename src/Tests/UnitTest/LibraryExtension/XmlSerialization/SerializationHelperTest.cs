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
            var e = new XElement("e", 103.Serialize("a"));
            Assert.AreEqual("103", e.Element("a").Value);
        }

        [Test]
        public void GetStringTest()
        {
            var e = XElement.Parse("<e><key>hello</key></e>");
            Assert.AreEqual("hello", e.GetString("key"));
        }

        [Test]
        public void GetIntTest()
        {
            var e = XElement.Parse("<e><key>-84</key></e>");
            Assert.AreEqual(-84, e.GetInt("key"));
        }
    }
}
