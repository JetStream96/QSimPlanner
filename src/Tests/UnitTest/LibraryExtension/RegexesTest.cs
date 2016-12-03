using NUnit.Framework;
using static QSP.LibraryExtension.Regexes;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class RegexesTest
    {
        [Test]
        public void PatternWithoutEmptyString()
        {
            string[] list = {"A", "BC", "DEF"};
            Assert.AreEqual( "(A|BC|DEF)", PatternMatchAny(list));
        }

        [Test]
        public void PatternWithEmptyString()
        {
            string[] list = {"A", "B", "", "C"};
            Assert.AreEqual( "(A|B|C)?", PatternMatchAny(list));
        }
    }
}