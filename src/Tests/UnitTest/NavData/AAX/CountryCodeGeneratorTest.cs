using CommonLibrary.LibraryExtension;
using NUnit.Framework;
using QSP.NavData.AAX;

namespace UnitTest.NavData.AAX
{
    [TestFixture]
    public class CountryCodeGeneratorTest
    {
        [Test]
        public void EmptyStringShouldReturnDefaultValue()
        {
            var gen = new CountryCodeGenerator(-1);
            Assert.AreEqual(-1, gen.GetCountryCode(""));
        }

        [Test]
        public void GenerateTest()
        {
            var gen = new CountryCodeGenerator(-1);
            Assert.AreEqual(1, gen.GetCountryCode("AA"));
            Assert.AreEqual(2, gen.GetCountryCode("BB"));
            Assert.AreEqual(1, gen.GetCountryCode("AA"));
            Assert.AreEqual(3, gen.GetCountryCode("CC"));
        }

        [Test]
        public void GenerateResult()
        {
            var gen = new CountryCodeGenerator(-1);
            gen.GetCountryCode("AA");
            gen.GetCountryCode("BB");
            gen.GetCountryCode("AA");
            gen.GetCountryCode("CC");

            var result = gen.CountryCodeLookup;
            Assert.IsTrue(result.FirstToSecond.Keys.SetEquals(1, 2, 3));
            Assert.IsTrue(result.FirstToSecond.Values.SetEquals("AA", "BB", "CC"));
        }

        [Test]
        public void CtorValidationTest()
        {
            Assert.DoesNotThrow(() => new CountryCodeGenerator(0));
            Assert.That(() => new CountryCodeGenerator(1), Throws.Exception);
        }
    }
}