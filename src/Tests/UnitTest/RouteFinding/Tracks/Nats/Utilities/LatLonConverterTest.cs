using NUnit.Framework;
using QSP.AviationTools.Coordinates;
using static QSP.RouteFinding.Tracks.Nats.Utilities.LatLonConverter;

namespace UnitTest.RouteFinding.Tracks.Nats.Utilities
{
    [TestFixture]
    public class LatLonConverterTest
    {
        [Test]
        public void WrongFormatShouldReturnFalse()
        {
            LatLon x;
            
            Assert.IsFalse(TryConvertNatsCoordinate("45/-20", out x));
        }

        [Test]
        public void CorrectFormatResultMatch()
        {
            Assert.IsTrue(ConvertNatsCoordinate("4550/20")
                .Equals(new LatLon(45.0 + 50.0 / 60.0, -20.0)));
            Assert.IsTrue(ConvertNatsCoordinate("45/20")
                .Equals(new LatLon(45.0, -20.0)));
            Assert.IsTrue(ConvertNatsCoordinate("4530.5/20")
                .Equals(new LatLon(45.0 + 30.5 / 60.0, -20.0)));
            Assert.IsTrue(ConvertNatsCoordinate("45/2045.5")
                .Equals(new LatLon(45.0, -(20.0 + 45.5 / 60.0))));
            Assert.IsTrue(ConvertNatsCoordinate("4530/2045.5")
                .Equals(new LatLon(45.0 + 30.0 / 60.0, -(20.0 + 45.5 / 60.0))));
        }

        [Test]
        public void AutoChooseFormatCorrectly()
        {
            Assert.IsTrue(new LatLon(50.0, -30.0).AutoChooseFormat() == "5030N");
            Assert.IsTrue(new LatLon(50.5, -30.0).AutoChooseFormat() == 
                "N50.500000W30.000000");
        }
    }
}
