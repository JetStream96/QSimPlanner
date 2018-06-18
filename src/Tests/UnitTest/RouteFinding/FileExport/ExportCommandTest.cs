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
            var command = new ExportCommand()
            {
                ProviderType = ProviderType.Fsx,
                Directory = @"C:\123",
                Enabled = true
            };
            var elem = command.Serialize("command1");
            var deserialized = ExportCommand.Deserialize(elem);

            Assert.IsTrue(command.Equals(deserialized));
        }
    }
}
