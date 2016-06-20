using NUnit.Framework;
using QSP.LibraryExtension;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class EqualityComparersTest
    {
        [Test]
        public void CreateTest()
        {
            var comparer = EqualityComparers.Create<string>(
                (x, y) => x.Length == y.Length);

            Assert.IsTrue(comparer.Equals("ABCD", "EFGH"));
            Assert.IsFalse(comparer.Equals("1", "12"));
        }
    }
}
