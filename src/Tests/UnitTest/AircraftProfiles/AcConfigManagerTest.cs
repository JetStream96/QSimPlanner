using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP;
using QSP.AircraftProfiles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.AircraftProfiles
{
    [TestClass]
    public class AcConfigManagerTest
    {
        private static AircraftConfig config1 =
            new AircraftConfig("B777-300ER",
                               "B-12345",
                               @"123/myConfig.xml",
                               @"456/myConfig.xml",
                               123456.0,
                               234567.0,
                               345678.0,
                               WeightUnit.KG);

        private static AircraftConfig config2 =
            new AircraftConfig("B777-300ER",
                               "B-9876",
                               @"a/myConfig.xml",
                               @"b/myConfig.xml",
                               23456.0,
                               34567.0,
                               45678.0,
                               WeightUnit.KG);

        [TestMethod]
        public void AddTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config1);
            manager.Add(config2);

            Assert.AreEqual(2, manager.Count);
        }

        [TestMethod]
        public void FindAircraftTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config1);
            manager.Add(config2);

            var result = manager.FindAircraft(config1.AC);

            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Contains(config1));
            Assert.IsTrue(result.Contains(config2));
        }

        [TestMethod]
        public void FindByRegistrationTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config1);

            var result = manager.FindRegistration(config1.Registration);

            Assert.IsNotNull(result);
            Assert.AreEqual(config1, result);
        }

        [TestMethod]
        public void RemoveTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config1);
            manager.Add(config2);
            manager.Remove(config1.Registration);

            Assert.AreEqual(1, manager.Count);

            var ac2 = manager.FindRegistration(config2.Registration);
            Assert.AreEqual(config2, ac2);
        }

        [TestMethod]
        public void ClearTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config1);
            manager.Clear();

            Assert.AreEqual(0, manager.Count);
        }

        [TestMethod]
        public void ValidateFileExistShouldPass()
        {
            var manager = new AcConfigManager();
            manager.Add(config1);

            var toFile =
                new QSP.TOPerfCalculation.Entry(config1.TOPerfFile, "", "", "");

            var toTable = new QSP.TOPerfCalculation.PerfTable(null, toFile);

            var ldgFile =
                new QSP.LandingPerfCalculation.Entry(config1.LdgPerfFile,
                                                     "", "", "");
            var ldgTable =
                new QSP.LandingPerfCalculation.PerfTable(null, ldgFile);

            manager.Validate(
                new List<QSP.TOPerfCalculation.PerfTable>() { toTable },
                new List<QSP.LandingPerfCalculation.PerfTable>() { ldgTable });
        }

        [TestMethod]
        [ExpectedException(typeof(PerfFileNotFoundException))]
        public void ValidateFileDoesNotExistShouldThrow()
        {
            var manager = new AcConfigManager();
            manager.Add(config1);

            manager.Validate(
                new List<QSP.TOPerfCalculation.PerfTable>(),
                new List<QSP.LandingPerfCalculation.PerfTable>());
        }
    }
}
