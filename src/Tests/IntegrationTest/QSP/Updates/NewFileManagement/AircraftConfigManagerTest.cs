using NUnit.Framework;
using QSP.Updates.NewFileManagement;
using System;
using System.IO;
using System.Reflection;
using static QSP.LibraryExtension.IOMethods;

namespace IntegrationTest.QSP.Updates.NewFileManagement
{
    [TestFixture]
    public class AircraftConfigManagerTest
    {
        private static readonly string FolderPath =
            "QSP/Updates/NewFileManagement/AircraftConfigManagerTesting";

        private static string TempDirectory()
        {
            return Path.Combine(FolderPath, "tmp");
        }

        private static string UpdaterAssemblyDirectory()
        {
            return Path.Combine(TempDirectory(), "0.3.0");
        }

        [Test]
        public void SetConfigTest()
        {
            Setup();
            Directory.SetCurrentDirectory(UpdaterAssemblyDirectory());

            var manager = new AircraftConfigManager(new Version("0.2.3"),
                new Version("0.3.0"));

            manager.SetConfigs();

            var acFolder = "PerformanceData/Aircrafts";

            // Old file is copied over.
            Assert.IsTrue(File.Exists(Path.Combine(
                acFolder, "B737-600_LN-RRX.xml")));

            Assert.IsTrue(File.Exists(Path.Combine(
                acFolder, "B737-700_B-123.xml")));

            // File not in the list of update.xml does not appear.
            Assert.IsFalse(File.Exists(Path.Combine(
                acFolder, "B737-700_B-5256.xml")));

            // Newly added file should be renamed to avoid collision.
            var newFile = Path.Combine(acFolder, "B737-700_B-123(1).xml");
            Assert.IsTrue(File.Exists(newFile));

            // Newly added config should have its registration renamed to 
            // avoid collision.
            Assert.IsTrue("B-123(1)" ==
                AircraftConfigManager.GetRegistration(newFile));
        }

        private void Setup()
        {
            var location = Assembly.GetExecutingAssembly().Location;
            Directory.SetCurrentDirectory(Path.GetDirectoryName(location));

            ClearDirectory(TempDirectory());
            CopyDirectory(Path.Combine(FolderPath, "files"), TempDirectory());
        }
    }
}
