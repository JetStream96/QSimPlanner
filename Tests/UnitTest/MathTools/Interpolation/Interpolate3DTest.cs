using Microsoft.VisualStudio.TestTools.UnitTesting;
using static QSP.MathTools.Interpolation.Interpolate3D;

namespace UnitTest.MathTools.Interpolation
{
    [TestClass]
    public class Interpolate3DTest
    {
        private double[][][] a = new double[][][] {
                                new double[][] {
                                    new double[] {-5.0,3.5,4.5},
                                    new double[] {-8.0,1.0,2.8},
                                    new double[] {-16.0,0.4,0.8}},
                                new double[][] {
                                    new double[] {2.0,18.5,14.5},
                                    new double[] {-0.8,14.0,12.8},
                                    new double[] {-1.0,11.0,4.8}} };

        [TestMethod]
        public void InterpolateTest1()
        {
            Assert.AreEqual(1.1607142857,
                            Interpolate(new double[] { 6.5, 10.5 },
                                        new double[] { 4.0, 5.0, 6.0 },
                                        new double[] { -1.0, 1.0, 8.0 },
                                        8.0, 6.5, 6.0, a),
                            1E-6);
        }

        [TestMethod]
        public void InterpolateTest2()
        {
            Assert.AreEqual(1.1607142857,
                            Interpolate(new double[] { 6.5, 10.5 }, 0,
                                        new double[] { 4.0, 5.0, 6.0 }, 1,
                                        new double[] { -1.0, 1.0, 8.0 }, 1,
                                        8.0, 6.5, 6.0, a),
                            1E-6);
        }

    }
}
