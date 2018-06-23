using NUnit.Framework;
using QSP.Common.Options;
using QSP.LibraryExtension;
using QSP.LibraryExtension.Sets;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;
using System.Linq;
using static QSP.LibraryExtension.Types;
using static QSP.RouteFinding.FileExport.Providers.Types;

namespace UnitTest.Common.Options
{
    [TestFixture]
    public class AppOptionTest
    {
        [Test]
        public void SerializationTest()
        {
            var command1 = new ExportCommand(ProviderType.Pmdg, @"C:\1", true, null);
            var command2 = new ExportCommand(ProviderType.Fsx, @"D:\1", false, null);

            var sims = Dict((SimulatorType.FSX_Steam, @"C:\FSX"),
                            (SimulatorType.Xplane11, @"C:\Xplane11"));

            var cmds = new ReadOnlySet<ExportCommand>(command1, command2);

            var option = new AppOptions("C:\\123", true, true, false, false, true,
                false, true, sims, cmds);

            var serializer = new AppOptions.Serializer();
            var elem = serializer.Serialize(option, "options");
            var deserialized = serializer.Deserialize(elem);

            var o = option;
            var d = deserialized;

            Assert.AreEqual(o.NavDataLocation, d.NavDataLocation);
            Assert.AreEqual(o.PromptBeforeExit, d.PromptBeforeExit);
            Assert.AreEqual(o.AutoDLTracks, d.AutoDLTracks);
            Assert.AreEqual(o.AutoDLWind, d.AutoDLWind);
            Assert.AreEqual(o.EnableWindOptimizedRoute, d.EnableWindOptimizedRoute);
            Assert.AreEqual(o.HideDctInRoute, d.HideDctInRoute);
            Assert.AreEqual(o.ShowTrackIdOnly, d.ShowTrackIdOnly);
            Assert.AreEqual(o.AutoUpdate, d.AutoUpdate);

            Assert.AreEqual(Enums.GetValues<SimulatorType>().Count(), d.SimulatorPaths.Count);
            Assert.AreEqual(@"C:\FSX", d.SimulatorPaths[SimulatorType.FSX_Steam]);
            Assert.AreEqual(@"C:\Xplane11", d.SimulatorPaths[SimulatorType.Xplane11]);

            Assert.AreEqual(DefaultExportCommands().Count, d.ExportCommands.Count);
            Assert.IsTrue(d.ExportCommands.Contains(command1));
            Assert.IsTrue(d.ExportCommands.Contains(command2));
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
