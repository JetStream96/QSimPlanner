using NUnit.Framework;
using QSP.Common.Options;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using System.Collections.Generic;

namespace UnitTest.Common.Options
{
    [TestFixture]
    public class AppOptionTest
    {
        [Test]
        public void SerializationTest()
        {
            var command1 = new ExportCommand(ProviderType.Pmdg, @"C:\1", true);
            var command2 = new ExportCommand(ProviderType.Fsx, @"D:\1", false);

            var cmds = new Dictionary<string, ExportCommand>()
            {
                ["PmdgNgx"] = command1,
                ["P3D"] = command2
            };

            var option = new AppOptions(
                "C:\\123", true, true, false, false, true, false, 0, cmds);

            var elem = option.Serialize("options");
            var deserialized = AppOptions.Deserialize(elem);

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
            Assert.IsTrue(d.ExportCommands["PmdgNgx"].Equals(command1));
            Assert.IsTrue(d.ExportCommands["P3D"].Equals(command2));
        }

        [Test]
        public void DeserializeMissingElementShouldContinue()
        {
            var option = AppOptions.Default;
            var elem = option.Serialize("options");
            elem.Element("AutoDLTracks").Remove();

            var deserialized = AppOptions.Deserialize(elem);

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
