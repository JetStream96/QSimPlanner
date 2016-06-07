using NUnit.Framework;
using static QSP.LibraryExtension.StringParser.Utilities;

namespace UnitTest.LibraryExtension.StringParser
{
    [TestFixture]
    public class UtilitiesTest
    {
        [Test]
        public void ParseIntTest()
        {
            int x;

            Assert.AreEqual(145689, ParseInt("as145689sad", 2, out x));
            Assert.AreEqual(7, x);

            Assert.AreEqual(145689, ParseInt("as145689", 2, out x));
            Assert.AreEqual(7, x);

            Assert.AreEqual(689, ParseInt("as145689", 5, out x));
            Assert.AreEqual(7, x);
        }
    }
}
