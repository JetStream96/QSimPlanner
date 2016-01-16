using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using QSP.RouteFinding.Tracks.Common;
using static QSP.LibraryExtension.Strings;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsMessage : TrackMessage<AusTrack>
    {
        public string AllText { get; private set; }

        public AusotsMessage(string HtmlSource)
        {
            AllText = HtmlSource.RemoveHtmlTags();
        }

        public override void LoadFromXml(XDocument doc)
        {
            AllText = doc.Root.Element("Text").Value;
        }

        public override string ToString()
        {
            return AllText;
        }

        public override XDocument ToXml()
        {
            var doc = new XElement("Content", new XElement[]{
                                                             new XElement("TrackSystem","AUSOTs"),
                                                             new XElement("Text",AllText)});
            return new XDocument(doc);
        }
    }
}
