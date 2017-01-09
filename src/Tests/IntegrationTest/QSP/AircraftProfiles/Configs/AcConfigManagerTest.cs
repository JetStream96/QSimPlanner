using NUnit.Framework;
using QSP.AircraftProfiles.Configs;
using QSP.FuelCalculation.FuelData;
using QSP.Utilities.Units;
using System.Collections.Generic;
using LdgCalc = QSP.LandingPerfCalculation;
using TOCalc = QSP.TOPerfCalculation;

namespace IntergrationTest.QSP.AircraftProfiles.Configs
{
    [TestFixture]
    public class AcConfigManagerTest
    {
        [Test]
        public void ValidateNoTOLdgPerfFileSelectedShouldPass()
        {
            var manager = new AcConfigManager();
            var txt = AircraftConfigItem.NoFuelTOLdgProfileText;

            manager.Add(
                new AircraftConfig(
                    new AircraftConfigItem("B777-300ER",
                        "B-12345",
                        txt,
                        txt,
                        txt,
                        123456.0,
                        234567.0,
                        345678.0,
                        456789.0,
                        567890.0,
                        1.0,
                        WeightUnit.KG),
                    "path"));

            manager.Validate(
                new FuelData[0], 
                new List<TOCalc.PerfTable>(),
                new List<LdgCalc.PerfTable>());
        }
    }
}
