using NUnit.Framework;
using static QSP.MathTools.Interpolation.Interpolate3D;

namespace UnitTest.MathTools.Interpolation
{
    [TestFixture]
    public class Interpolate3DTest
    {
        private double[][][] a = 
        {
            new[] 
            {
                new[] {-5.0,3.5,4.5},
                new[] {-8.0,1.0,2.8},
                new[] {-16.0,0.4,0.8}
            },
            new[] 
            {
                new[] {2.0,18.5,14.5},
                new[] {-0.8,14.0,12.8},
                new[] {-1.0,11.0,4.8}
            }
        };

        [Test]
        public void InterpolateTest()
        {
            Assert.AreEqual(1.1607142857,
                            Interpolate(new[] { 6.5, 10.5 },
                                        new[] { 4.0, 5.0, 6.0 },
                                        new[] { -1.0, 1.0, 8.0 }, a, 8.0, 6.5, 6.0),
                            1E-6);
        }
    }
}
