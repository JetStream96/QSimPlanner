using QSP.RouteFinding.Tracks.Common;
using System.Xml.Linq;
using static QSP.LibraryExtension.Strings;

namespace QSP.RouteFinding.Tracks.Ausots
{
    /// <summary>
    /// Contains all text of TDM (track definition message), 
    /// but with html tags removed.
    /// </summary>
    public class AusotsMessage : ITrackMessageNew
    {
        public static readonly string TrackSystem = "Ausots";
        public string AllText { get; private set; }

        public AusotsMessage(string HtmlSource)
        {
            AllText = HtmlSource.RemoveHtmlTags();
        }

        public AusotsMessage(XDocument doc)
        {
            LoadFromXml(doc);
        }

        private void LoadFromXml(XDocument doc)
        {
            AllText = doc.Root.Element("Text").Value;
        }

        public override string ToString()
        {
            return AllText;
        }

        public XDocument ToXml()
        {
            var doc = new XElement(
                "Content", new XElement[]{
                    new XElement("TrackSystem", TrackSystem),
                    new XElement("Text", AllText)});

            return new XDocument(doc);
        }
    }
}
