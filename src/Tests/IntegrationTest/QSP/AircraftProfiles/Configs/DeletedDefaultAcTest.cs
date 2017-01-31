using IntegrationTest.Util;
using NUnit.Framework;
using QSP.AircraftProfiles.Configs;
using System.IO;
using System.Linq;
using static QSP.LibraryExtension.IEnumerables;

namespace IntegrationTest.QSP.AircraftProfiles.Configs
{
    [TestFixture, SingleThreaded]
    public class DeletedDefaultAcTest
    {
        private string FullPath => Path.Combine(CommonUtil.AssemblyDirectory(),
            "QSP/AircraftProfiles/Configs/deleted.xml");

        private readonly string text = @"
<Root>
    <Item>A-BCDE</Item>
    <Item>N331D</Item>
</Root>";

        private readonly string emptyText = "<Root></Root>";

        [Test]
        public void ReadTest()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            File.WriteAllText(FullPath, text);

            var deleted = new DeletedDefaultAc(FullPath);
            Assert.IsTrue(deleted.DeletedRegistration().SequenceEqual("A-BCDE", "N331D"));
        }

        [Test]
        public void ReadEmptyFileTest()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            File.WriteAllText(FullPath, emptyText);

            var deleted = new DeletedDefaultAc(FullPath);
            Assert.AreEqual(0, deleted.DeletedRegistration().Count());
        }

        [Test]
        public void ReadNoFileTest()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            File.Delete(FullPath);

            var deleted = new DeletedDefaultAc(FullPath);
            Assert.AreEqual(0, deleted.DeletedRegistration().Count());
        }

        [Test]
        public void AddThenRead()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            File.WriteAllText(FullPath, text);

            var deleted = new DeletedDefaultAc(FullPath);
            deleted.Add("N567S");
            Assert.IsTrue(deleted.DeletedRegistration().SequenceEqual("A-BCDE", "N331D", "N567S"));
        }

        [Test]
        public void NoFileAddThenRead()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FullPath));
            File.Delete(FullPath);

            var deleted = new DeletedDefaultAc(FullPath);
            deleted.Add("N567S");
            Assert.IsTrue(deleted.DeletedRegistration().SequenceEqual("N567S"));
        }
    }
}