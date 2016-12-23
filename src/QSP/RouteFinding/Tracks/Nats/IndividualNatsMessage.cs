using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class IndividualNatsMessage
    {
        public string LastUpdated { get; private set; }
        public string Header { get; private set; }
        public NatsDirection Direction { get; private set; }
        public string Message { get; private set; }

        public IndividualNatsMessage(
            string LastUpdated,
            string Header,
            NatsDirection Direction,
            string Message)
        {
            this.LastUpdated = LastUpdated;
            this.Header = Header;
            this.Direction = Direction;
            this.Message = Message;
        }

        /// <summary>
        /// Create an instance of NatsMessage from an xml file.
        /// </summary>
        public IndividualNatsMessage(XDocument doc) : this(doc.Root) { }

        public IndividualNatsMessage(XElement elem)
        {
            LastUpdated = elem.Element("LastUpdated").Value;
            Header = elem.Element("Header").Value;
            Message = elem.Element("Message").Value;

            string s = elem.Element("Direction").Value;

            Direction = s == "East" ? NatsDirection.East : NatsDirection.West;
        }

        private string NatsDirectionString()
        {
            return Direction == NatsDirection.East ? "East" : "West";
        }

        public XDocument ConvertToXml()
        {
            return new XDocument(
                new XElement( "Content",
                    new XElement[] {
                        new XElement("LastUpdated", LastUpdated),
                        new XElement("Header", Header),
                        new XElement("Direction", NatsDirectionString()),
                        new XElement("Message", Message)}));
        }
    }
}

