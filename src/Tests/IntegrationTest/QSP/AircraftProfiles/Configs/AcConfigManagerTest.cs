using NUnit.Framework;
using QSP.AircraftProfiles.Configs;
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
            var txt = AircraftConfigItem.NoTOLdgProfileText;

            manager.Add(
                new AircraftConfig(
                    new AircraftConfigItem("B777-300ER",
                                       "B-12345",
                                       txt,
                                       txt,
                                       123456.0,
                                       234567.0,
                                       345678.0,
                                       WeightUnit.KG),
                    "path"));

            manager.Validate(
                new List<TOCalc.PerfTable>(),
                new List<LdgCalc.PerfTable>());
        }
    }
}
