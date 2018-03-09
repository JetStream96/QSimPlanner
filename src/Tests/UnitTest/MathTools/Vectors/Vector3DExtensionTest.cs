using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using QSP.MathTools.Vectors;

namespace UnitTest.MathTools.Vectors
{
    [TestFixture]
    public class Vector3DExtensionTest
    {
        [Test]
        public void ToVector3DTest()
        {
            var delta = 1e-7;
            Assert.IsTrue(new LatLon(0, 0).ToVector3D().Equals(new Vector3D(1, 0, 0), delta));
            Assert.IsTrue(new LatLon(90, 8).ToVector3D().Equals(new Vector3D(0, 0, 1), delta));
            Assert.IsTrue(new LatLon(0, 180).ToVector3D().Equals(new Vector3D(-1, 0, 0), delta));
            Assert.IsTrue(new LatLon(0, 90).ToVector3D().Equals(new Vector3D(0, 1, 0), delta));
        }
    }
}