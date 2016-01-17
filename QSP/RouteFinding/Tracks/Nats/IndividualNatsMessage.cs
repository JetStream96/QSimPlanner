using System;
using System.Collections.Generic;
using System.Xml.Linq;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Nats
{    
    public class IndividualNatsMessage
    {
        public string LastUpdated { get; private set; }
        public string Header { get; private set; }
        public NatsDirection Direction { get; private set; }
        public string Message { get; private set; }

        public IndividualNatsMessage(string LastUpdated, string Header, NatsDirection Direction, string Message)
        {
            this.LastUpdated = LastUpdated;
            this.Header = Header;
            this.Direction = Direction;
            this.Message = Message;
        }

        /// <summary>
        /// Create an instance of NATsMessage from an xml file.
        /// </summary>
        public IndividualNatsMessage(XDocument doc) : this(doc.Root) { }
    
        public IndividualNatsMessage(XElement elem)
        {
            LastUpdated = elem.Element("LastUpdated").Value;
            Header = elem.Element("Header").Value;
            Message = elem.Element("Message").Value;

            string s = elem.Element("Direction").Value;

            Direction = (s == "East") ? NatsDirection.East : NatsDirection.West;
        }

        private string NatsDirectionString()
        {
            if (Direction == NatsDirection.East)
            {
                return "East";
            }
            else
            {
                return "West";
            }
        }

        public XDocument ConvertToXml()
        {
            return new XDocument(
                        new XElement("Content", new XElement[] {
                                                    new XElement("LastUpdated", LastUpdated),
                                                    new XElement("Header", Header),
                                                    new XElement("Direction", NatsDirectionString()),
                                                    new XElement("Message", Message)}));
        }

        public List<NorthAtlanticTrackOld> ConvertToTracks()
        {
            char trkStartChar = (Direction == NatsDirection.West ? 'A' : 'N');

            var tracks = new List<NorthAtlanticTrackOld>();

            for (int i = trkStartChar; i <= trkStartChar + 12; i++)
            {
                int j = Message.IndexOf("\n" + (char)i + " ");

                if (j == -1)
                {
                    continue;
                }

                int k = Message.IndexOf('\n', j + 3);

                string str = Message.Substring(j + 3, k - j - 3);
                string[] wp = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                tracks.Add(new NorthAtlanticTrackOld(Direction, (char)i, new List<string>(wp), new List<int>()));
            }

            return tracks;
        }

    }

}

