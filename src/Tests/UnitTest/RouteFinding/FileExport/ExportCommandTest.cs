using NUnit.Framework;
using QSP.RouteFinding.FileExport;
using QSP.RouteFinding.FileExport.Providers;

namespace UnitTest.RouteFinding.FileExport
{
    [TestFixture]
    public class ExportCommandTest
    {
        [Test]
        public void SerializeTest()
        {
            var command = new ExportCommand(ProviderType.Fsx, @"C:\123", true, null);

            var elem = command.Serialize("command1");
            var deserialized = ExportCommand.Deserialize(elem);

            Assert.IsTrue(command.Equals(deserialized));
        }
    }
}
