using NUnit.Framework;
using static QSP.MathTools.Interpolation.Interpolate2D;

namespace UnitTest.MathTools.Interpolation
{
    [TestFixture]
    public class Interpolate2DTest
    {
        private double[][] a = 
        {
            new[] {-5.0,3.5,4.5},
            new[] {-8.0,1.0,2.8},
            new[] {-16.0,0.4,0.8}
        };

        [Test]
        public void InterpolateTest()
        {
            Assert.AreEqual(-0.114285714,
                            Interpolate(new[] { 4.0, 5.0, 6.0 },
                                        new[] { -1.0, 1.0, 8.0 },
                                        6.5, 6.0, a),
                            1E-6);
        }
    }
}
