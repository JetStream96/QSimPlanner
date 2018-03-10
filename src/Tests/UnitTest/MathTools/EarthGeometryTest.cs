using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.MathTools.Vectors;
using static QSP.MathTools.EarthGeometry;
using static System.Math;

namespace UnitTest.MathTools
{
    [TestFixture]
    public class EarthGeometryTest
    {
        [Test]
        public void TureHeadingTest()
        {
            var delta = 1e-7;

            var p0 = new LatLon(0, 0);
            var p1 = new LatLon(0, 90);
            var p2 = new LatLon(-90, 6);
            var p3 = new LatLon(45, 0);

            var v0 = new Vector3D(15, 0, 0);
            var v1 = new Vector3D(0, 0, 0.3);
            var v2 = new Vector3D(0, 1, 0);
            var v3 = new Vector3D(0, 1, 1);

            bool WithinRange(double d) => 0 < d && d <= 360;
            bool IsZero(double d) => Abs(d) <= delta || Abs(d - 360) <= delta;

            var r = TrueHeading(v0, p0);
            Assert.IsTrue(WithinRange(TrueHeading(v0, p0)));
            Assert.IsTrue(IsZero(TrueHeading(v1, p0)));
            Assert.AreEqual(90, TrueHeading(v2, p0), delta);
            Assert.AreEqual(45, TrueHeading(v3, p0), delta);
            Assert.AreEqual(270, TrueHeading(v0, p1), delta);
            Assert.IsTrue(IsZero(TrueHeading(v0, p2)));
            Assert.IsTrue(IsZero(TrueHeading(v1, p3)));
        }

        [Test]
        public void TureHeadingCoordinateTest()
        {
            Assert.AreEqual(90, TrueHeading(new LatLon(0, 20), new LatLon(0, 60)), 1e-7);
        }
    }
}