using NUnit.Framework;
using QSP.LibraryExtension;

namespace UnitTest.LibraryExtension
{
    [TestFixture]
    public class IEnumerablesTest
    {
        [Test]
        public void MaxByTest()
        {
            var x = new double[] { 5.0, 3.0, -8.0 };

            Assert.AreEqual(-8.0, x.MaxBy(t => t * t));
        }
    }
}
