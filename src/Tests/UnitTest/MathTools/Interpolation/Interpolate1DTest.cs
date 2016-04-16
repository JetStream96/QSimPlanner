using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static QSP.MathTools.Interpolation.Interpolate1D;

namespace UnitTest.MathTools.Interpolation
{
    [TestClass]
    public class Interpolate1DTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void X0X1AreTheSameThrowException()
        {
            double val = Interpolate(10.0, 10.0, 3, 5, 10.0);
        }

        [TestMethod]
        public void X0X1AreTheSameTrivialCase()
        {
            Assert.AreEqual(5.0, Interpolate(10.0, 10.0, 5.0, 5.0, 10.0), 1E-6);
        }

        [TestMethod]
        public void InterpolateTest1()
        {
            Assert.AreEqual(-4.0, Interpolate(3.0, 5.0, -8.0, 8.0, 3.5), 1E-6);
        }

        [TestMethod]
        public void InterpolateTest2()
        {
            Assert.AreEqual(12.2,
                            Interpolate(new double[] { 3.0, 5.0, 7.0, 9.0 },
                                        new double[] { -9.0, -1.0, 12.0, 14.0 },
                                        7.2),
                            1E-6);
        }

        [TestMethod]
        public void InterpolateTest3()
        {
            Assert.AreEqual(12.2,
                            Interpolate(new double[] { 3.0, 5.0, 7.0, 9.0 },
                                        new double[] { -9.0, -1.0, 12.0, 14.0 },
                                        2,
                                        7.2),
                            1E-6);
        }

    }
}
