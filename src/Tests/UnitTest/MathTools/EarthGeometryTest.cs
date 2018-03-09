using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.MathTools;
using QSP.MathTools.Vectors;

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

            var v0 = new Vector3D(15, 0, 0);
            var v1 = new Vector3D(0, 0, 0.3);
            var v2 = new Vector3D(0, 1, 0);
            var v3 = new Vector3D(0, 1, 1);

            var r = EarthGeometry.TrueHeading(v0, p0);
            Assert.IsTrue(0 < r && r <= 360);
            Assert.AreEqual(360, EarthGeometry.TrueHeading(v1, p0), delta);
            Assert.AreEqual(90, EarthGeometry.TrueHeading(v2, p0), delta);
            Assert.AreEqual(45, EarthGeometry.TrueHeading(v3, p0), delta);
            Assert.AreEqual(270, EarthGeometry.TrueHeading(v0, p1), delta);
            Assert.AreEqual(360, EarthGeometry.TrueHeading(v0, p2), delta);
        }
    }
}