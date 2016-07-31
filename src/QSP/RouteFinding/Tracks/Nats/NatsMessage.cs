using QSP.RouteFinding.Tracks.Common;
using System.Text;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsMessage : TrackMessage
    {
        public override string TrackSystem { get { return "Nats"; } }
        public IndividualNatsMessage WestMessage { get; private set; }
        public IndividualNatsMessage EastMessage { get; private set; }

        public NatsMessage(IndividualNatsMessage WestMessage,
            IndividualNatsMessage EastMessage)
        {
            this.WestMessage = WestMessage;
            this.EastMessage = EastMessage;
        }

        public override void LoadFromXml(XDocument doc)
        {
            var root = doc.Root;
            WestMessage = new IndividualNatsMessage(root.Element("Westbound"));
            EastMessage = new IndividualNatsMessage(root.Element("Eastbound"));
        }

        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append(WestMessage.Header);
            s.Append($"\n\nWestbound tracks ({WestMessage.LastUpdated}):\n\n");
            s.Append(WestMessage.Message);
            s.Append($"\n\nEastbound tracks ({EastMessage.LastUpdated}):\n\n");
            s.Append(EastMessage.Message);
            return s.ToString();
        }

        public override XDocument ToXml()
        {
            var west = WestMessage.ConvertToXml().Root;
            var east = EastMessage.ConvertToXml().Root;

            return new XDocument(
                new XElement("Content",
                    new XElement("TrackSystem", TrackSystem),
                    new XElement[] {
                        new XElement("Westbound", west),
                        new XElement("Eastbound", east)}));
        }
    }
}
