using NUnit.Framework;
using QSP.RouteFinding.Tracks.Interaction;
using QSP.RouteFinding.Tracks.Common;

namespace UnitTest.RouteFinding.Tracks.Interaction
{
    [TestFixture]
    public class StatusRecorderTest
    {
        [Test]
        public void NewRecorderShouldBeEmpty()
        {
            var sr = new StatusRecorder();
            Assert.AreEqual(0, sr.Records.Count);
        }

        [Test]
        public void AddRecordTest()
        {
            var sr = new StatusRecorder();
            sr.AddEntry(StatusRecorder.Severity.Caution, "1", TrackType.Pacots);

            var items = sr.Records;
            Assert.AreEqual(1, items.Count);
            Assert.IsTrue(items[0].Severity == StatusRecorder.Severity.Caution);
            Assert.IsTrue(items[0].Message == "1");
            Assert.IsTrue(items[0].Type == TrackType.Pacots);
        }

        [Test]
        public void AfterClearShouldBeEmpty()
        {
            var sr = new StatusRecorder();
            sr.AddEntry(StatusRecorder.Severity.Caution, "1", TrackType.Pacots);
            sr.AddEntry(StatusRecorder.Severity.Critical, "2", TrackType.Nats);

            sr.Clear();

            Assert.AreEqual(0, sr.Records.Count);
        }
    }
}
