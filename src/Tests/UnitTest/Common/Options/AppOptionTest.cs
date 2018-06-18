using NUnit.Framework;
using QSP.Common.Options;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using System.Collections.Generic;
using static QSP.LibraryExtension.Types;

namespace UnitTest.Common.Options
{
    [TestFixture]
    public class AppOptionTest
    {
        [Test]
        public void SerializationTest()
        {
            var command1 = new ExportCommand()
            {
                ProviderType = ProviderType.Pmdg,
                Directory = @"C:\1",
                Enabled = true
            };

            var command2 = new ExportCommand()
            {
                ProviderType = ProviderType.Fsx,
                Directory = @"D:\1",
                Enabled = false
            };

            var paths = Dict(("FSX", @"C:\FSX"), ("P3D", @"C:\P3D"));
            var cmds = Dict(("PmdgNgx", command1), ("P3D", command2));

            var option = new AppOptions(
                "C:\\123", true, true, false, false, true, false, true, paths, cmds);

            var serializer = new AppOptions.Serializer();
            var elem = serializer.Serialize(option, "options");
            var deserialized = serializer.Deserialize(elem);

            var o = option;
            var d = deserialized;

            Assert.AreEqual(o.NavDataLocation, d.NavDataLocation);
            Assert.AreEqual(o.PromptBeforeExit, d.PromptBeforeExit);
            Assert.AreEqual(o.AutoDLTracks, d.AutoDLTracks);
            Assert.AreEqual(o.AutoDLWind, d.AutoDLWind);
            Assert.AreEqual(
                o.EnableWindOptimizedRoute, d.EnableWindOptimizedRoute);
            Assert.AreEqual(o.HideDctInRoute, d.HideDctInRoute);
            Assert.AreEqual(o.ShowTrackIdOnly, d.ShowTrackIdOnly);
            Assert.AreEqual(o.AutoUpdate, d.AutoUpdate);
            Assert.AreEqual(cmds.Count, d.ExportCommands.Count);
            Assert.AreEqual(paths["FSX"], d.SimulatorPaths["FSX"]);
            Assert.AreEqual(paths["P3D"], d.SimulatorPaths["P3D"]);
            Assert.IsTrue(d.ExportCommands["PmdgNgx"].Equals(command1));
            Assert.IsTrue(d.ExportCommands["P3D"].Equals(command2));
        }

        [Test]
        public void DeserializeMissingElementShouldContinue()
        {
            var option = AppOptions.Default;
            var serializer = new AppOptions.Serializer();
            var elem = serializer.Serialize(option, "options");
            elem.Element("AutoDLTracks").Remove();

            var deserialized = serializer.Deserialize(elem);

            var o = option;
            var d = deserialized;

            Assert.AreEqual(o.NavDataLocation, d.NavDataLocation);
            Assert.AreEqual(o.PromptBeforeExit, d.PromptBeforeExit);
            // Assert.AreEqual(o.AutoDLTracks, d.AutoDLTracks);
            Assert.AreEqual(o.AutoDLWind, d.AutoDLWind);
            Assert.AreEqual(
                o.EnableWindOptimizedRoute, d.EnableWindOptimizedRoute);
            Assert.AreEqual(o.HideDctInRoute, d.HideDctInRoute);
            Assert.AreEqual(o.ShowTrackIdOnly, d.ShowTrackIdOnly);
            Assert.AreEqual(o.AutoUpdate, d.AutoUpdate);
            Assert.AreEqual(0, d.ExportCommands.Count);
        }
    }
}
