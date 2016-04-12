using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP;
using QSP.AircraftProfiles;
using System;

namespace UnitTest.AircraftProfiles
{
    [TestClass]
    public class AcConfigManagerTest
    {
        private static AircraftConfig config =
            new AircraftConfig("B777-300ER",
                               "B-12345",
                               @"123/myConfig.xml",
                               @"456/myConfig.xml",
                               123456.0,
                               234567.0,
                               345678.0,
                               WeightUnit.KG);

        [TestMethod]
        public void AddAndFindByAcTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config);

            var result = manager.Find(config.AC);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(config, result[0]);
        }

        [TestMethod]
        public void AddAndFindByAcAndRegistrationTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config);

            var result = manager.Find(config.AC, config.Registration);

            Assert.IsNotNull(result);
            Assert.AreEqual(config, result);
        }

        [TestMethod]
        public void AddAndRemoveTest()
        {
            var manager = new AcConfigManager();
            manager.Add(config);
            manager.Remove(config.AC, config.Registration);
            // TODO:
        }
    }
}
