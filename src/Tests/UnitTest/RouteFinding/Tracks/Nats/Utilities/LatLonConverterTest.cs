using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.AviationTools.Coordinates;
using static QSP.RouteFinding.Tracks.Nats.Utilities.LatLonConverter;

namespace UnitTest.RouteFinding.Tracks.Nats.Utilities
{
    [TestClass]
    public class LatLonConverterTest
    {
        [TestMethod]
        public void WrongFormatShouldReturnFalse()
        {
            LatLon x;

            Assert.IsFalse(TryConvertNatsCoordinate("4570/20", out x));
            Assert.IsFalse(TryConvertNatsCoordinate("45/-20", out x));
        }

        [TestMethod]
        public void CorrectFormatResultMatch()
        {
            Assert.IsTrue(ConvertNatsCoordinate("4550/20").Equals(new LatLon(45.0 + 50.0 / 60, -20.0)));
            Assert.IsTrue(ConvertNatsCoordinate("45/20").Equals(new LatLon(45.0, -20.0)));
            Assert.IsTrue(ConvertNatsCoordinate("4530.5/20").Equals(new LatLon(45.0 + 30.5 / 60, -20.0)));
            Assert.IsTrue(ConvertNatsCoordinate("45/2045.5").Equals(new LatLon(45.0, -(20.0 + 45.5 / 60))));
            Assert.IsTrue(ConvertNatsCoordinate("4530/2045.5").Equals(new LatLon(45.0 + 30.0 / 60, -(20.0 + 45.5 / 60))));
        }

        [TestMethod]
        public void AutoChooseFormatCorrectly()
        {
            Assert.IsTrue(new LatLon(50.0, -30.0).AutoChooseFormat() == "5030N");
            Assert.IsTrue(new LatLon(50.5, -30.0).AutoChooseFormat() == "N50.500000W30.000000");
        }
    }
}
