using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QSP.RouteFinding.Tracks.Ausots;
using System.Xml.Linq;

namespace Tests.RouteFinding.Tracks.Ausots
{
    [TestClass]
    public class AusotsMessageTest
    {
        [TestMethod]
        public void WhenCallToStringHtmlTagShouldNotAppear()
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

        [TestMethod]
        public void WhenCallToXmlFormatShouldBeCorrect()
        {
            var msg = new AusotsMessage(
@"<pre>
TDM should appear here.
</pre>");

            var xml = msg.ToXml();

            Assert.IsTrue(xml.Root.Element("TrackSystem").Value == "AUSOTs");
            Assert.IsTrue(xml.Root.Element("Text").Value == msg.AllText);
        }

        [TestMethod]
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
