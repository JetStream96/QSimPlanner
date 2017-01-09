using NUnit.Framework;
using static QSP.LibraryExtension.English;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class EnglishTest
    {
        [Test]
        public void CombineTest()
        {
            Assert.AreEqual("a", Combined("a"));
            Assert.AreEqual("a and b", Combined("a", "b"));
            Assert.AreEqual("a, b and c", Combined("a", "b", "c"));
        }
    }
}