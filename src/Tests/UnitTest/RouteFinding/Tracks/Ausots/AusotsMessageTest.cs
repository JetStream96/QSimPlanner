using NUnit.Framework;
using QSP.RouteFinding.Tracks.Ausots;
using System.Xml.Linq;
using QSP.RouteFinding.Tracks.Common;

namespace UnitTest.RouteFinding.Tracks.Ausots
{
    [TestFixture]
    public class AusotsMessageTest
    {
        [Test]
        public void ToStringHtmlTagShouldNotAppear()
        {
            var msg = new AusotsMessage(
@"<pre>
TDM should appear here.
</pre>");

            Assert.IsTrue(msg.ToString() ==
@"
TDM should appear here.
");
        }

        [Test]
        public void ToXmlFormatShouldBeCorrect()
        {
            var msg = new AusotsMessage(
@"<pre>
TDM should appear here.
</pre>");

            var xml = msg.ToXml();
            var trackSys = xml.Root.Element("TrackSystem").Value;
            Assert.IsTrue(trackSys == TrackType.Ausots.TrackString());
            Assert.IsTrue(xml.Root.Element("Text").Value == msg.AllText);
        }

        [Test]
        public void LoadFromXmlAcceptsCorrectFormat()
        {
            var doc = XDocument.Parse(
@"<Content>
    <TrackSystem>AUSOTs</TrackSystem>
    <Text>My TDM should be here.</Text>
  </Content>");

            var msg = new AusotsMessage(doc);
            Assert.IsTrue(msg.AllText == "My TDM should be here.");
        }
    }
}
