using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.MathTools;
using static QSP.MathTools.Interpolation;

namespace UnitTest
{

    [TestClass()]
    public class InterpolationTest
    {

        [TestMethod()]

        public void InterpolateTest2DDelegate()
        {

            double[] xArr = { 0.0, 10.0, 20.0 };
            double[] yArr = { 35.0, 55.0, 75.0 };

            Assert.AreEqual(975.0, Interpolation.Interpolate(xArr, yArr, 15, 65, (a, b) => xArr[a] * yArr[b],
                ArrayOrder.Increasing, ArrayOrder.Increasing));

        }

        [TestMethod()]

        public void Interpolate3DTest()
        {
            double[] xArr = { 0.0, -10.0, -20.0 };
            double[] yArr = { 35.0, 55.0, 75.0 };
            double[] zArr = { 0.0, 1.0 };

            double[,,] f = {
                {
                    {-1,   15,   25},
                {-8,   -99,   50},
                {25,   100,   -6}},
                {
                    {9,   5,   1},
                {615,   -6,   -5},
                {50,   30,   40}}};

            Assert.AreEqual(106.9375, Interpolation.Interpolate(xArr, yArr, zArr, -5, 60, 0.5, f, ArrayOrder.Decreasing, ArrayOrder.Increasing, ArrayOrder.Increasing));

        }

    }
}
