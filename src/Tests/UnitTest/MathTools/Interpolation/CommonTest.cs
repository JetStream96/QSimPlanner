using Microsoft.VisualStudio.TestTools.UnitTesting;
using static QSP.MathTools.Interpolation.Common;

namespace UnitTest.MathTools.Interpolation
{
    [TestClass]
    public class CommonTest
    {
        [TestMethod]
        public void GetIndexIncreasingTest()
        {
            double[] a = new double[] { 3.0, 4.0, 5.0, 6.0, 8.0 };

            Assert.AreEqual(0, GetIndex(a, -1.5));
            Assert.AreEqual(2, GetIndex(a, 5.5));
            Assert.AreEqual(3, GetIndex(a, 15.0));
        }

        [TestMethod]
        public void GetIndexDecreasingTest()
        {
            double[] a = new double[] { 35.0, 21.0, 10.0, 5.0, -2.0 };

            Assert.AreEqual(0, GetIndex(a, 45.5));
            Assert.AreEqual(2, GetIndex(a, 5.5));
            Assert.AreEqual(3, GetIndex(a, -15.0));
        }
    }
}
