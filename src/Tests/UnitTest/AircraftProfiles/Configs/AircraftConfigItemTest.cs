using NUnit.Framework;
using QSP.AircraftProfiles.Configs;
using QSP.Utilities.Units;

namespace UnitTest.AircraftProfiles.Configs
{
    [TestFixture]
    public class AircraftConfigItemTest
    {
        [Test]
        public void SerializationTest()
        {
            var config = new AircraftConfigItem("A", "B", "C", "D", "E",
                1.0, 2.0, 3.0, 4.0, 5.0, WeightUnit.LB);

            var elem = config.Serialize("Config");
            var deserialized = AircraftConfigItem.Deserialize(elem);

            Assert.IsTrue(config.Equals(deserialized, 0.0));
        }
    }
}
