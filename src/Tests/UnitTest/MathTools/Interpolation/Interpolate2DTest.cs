using NUnit.Framework;
using static QSP.MathTools.Interpolation.Interpolate2D;

namespace UnitTest.MathTools.Interpolation
{
    [TestFixture]
    public class Interpolate2DTest
    {
        private double[][] a = new double[][] {
                                new double[] {-5.0,3.5,4.5},
                                new double[] {-8.0,1.0,2.8},
                                new double[] {-16.0,0.4,0.8}};

        [Test]
        public void InterpolateTest1()
        {
            Assert.AreEqual(-0.114285714,
                            Interpolate(new double[] { 4.0, 5.0, 6.0 },
                                        new double[] { -1.0, 1.0, 8.0 },
                                        6.5, 6.0, a),
                            1E-6);
        }

        [Test]
        public void InterpolateTest2()
        {
            Assert.AreEqual(-0.114285714,
                            Interpolate(new double[] { 4.0, 5.0, 6.0 }, 1,
                                        new double[] { -1.0, 1.0, 8.0 }, 1,
                                        6.5, 6.0, a),
                            1E-6);
        }

    }
}
