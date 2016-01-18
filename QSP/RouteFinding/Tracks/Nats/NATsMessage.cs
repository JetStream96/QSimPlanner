using System.Text;
using System.Xml.Linq;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsMessage : TrackMessage<NorthAtlanticTrack>
    {
        public IndividualNatsMessage WestMessage { get; private set; }
        public IndividualNatsMessage EastMessage { get; private set; }

        public NatsMessage(IndividualNatsMessage WestMessage, IndividualNatsMessage EastMessage)
        {
            this.WestMessage = WestMessage;
            this.EastMessage = EastMessage;
        }

        public NatsMessage(XDocument doc)
        {
            LoadFromXml(doc);
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
            s.AppendLine(WestMessage.Header);
            s.AppendLine(WestMessage.LastUpdated);
            s.AppendLine(WestMessage.Message);

            s.AppendLine(EastMessage.LastUpdated);
            s.AppendLine(EastMessage.Message);

            return s.ToString();
        }

        public override XDocument ToXml()
        {
            return new XDocument(new XElement("Content", new XElement[] { new XElement("Westbound",WestMessage.ConvertToXml().Root),
                                                                          new XElement("Eastbound",EastMessage.ConvertToXml().Root)}));
        }
    }
}
