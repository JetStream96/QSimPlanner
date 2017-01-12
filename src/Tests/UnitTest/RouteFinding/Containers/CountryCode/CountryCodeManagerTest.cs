using NUnit.Framework;
using QSP.LibraryExtension;
using QSP.RouteFinding.Containers.CountryCode;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.RouteFinding.Containers.CountryCode
{
    [TestFixture]
    public class CountryCodeManagerTest
    {
        [Test]
        public void CtorShouldValidate()
        {
            var codeLetterLookup = new BiDictionary<int, string>();
            codeLetterLookup.Add(0, "AA");
            codeLetterLookup.Add(1, "BB");

            var fullNameLookup = new Dictionary<string, string>()
            {
                {"AA", "Country A"},
                {"XX", "Country X"}
            };

            var manager = new CountryCodeManager(codeLetterLookup, fullNameLookup);

            Assert.AreEqual(2, manager.CodeToLetterLookup.Count);
            Assert.IsTrue(manager.CodeToLetterLookup.Contains(
                new KeyValuePair<int, string>(0, "AA")));
            Assert.IsTrue(manager.CodeToLetterLookup.Contains(
                new KeyValuePair<int, string>(1, "BB")));

            Assert.AreEqual(2, manager.FullNameLookup.Count);
            Assert.IsTrue(manager.FullNameLookup.Contains(
                new KeyValuePair<string, string>("AA", "Country A")));
            Assert.IsTrue(manager.FullNameLookup.ContainsKey("BB"));
        }
    }
}
