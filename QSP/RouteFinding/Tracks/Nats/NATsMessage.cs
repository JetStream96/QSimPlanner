using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Nats
{
    public enum NatsDir
    {
        East,
        West
    }

    public class NATsMessage
    {
        public string LastUpdated { get; private set; }
        public string GeneralInfo { get; private set; }
        public NatsDir Direction { get; private set; }
        public string Message { get; private set; }

        public NATsMessage(string LastUpdated, string GeneralInfo, NatsDir Direction, string Message)
        {
            this.LastUpdated = LastUpdated;
            this.GeneralInfo = GeneralInfo;
            this.Direction = Direction;
            this.Message = Message;
        }

        /// <summary>
        /// Create an instance of NATsMessage from an xml file.
        /// </summary>
        /// <param name="str">Contents in xml</param>

        public NATsMessage(string str)
        {
            var root = XDocument.Parse(str).Root;

            LastUpdated = root.Element("LastUpdated").Value;
            GeneralInfo = root.Element("GeneralInfo").Value;
            Message = root.Element("Message").Value;

            string s = root.Element("Direction").Value;

            if (s == "East")
            {
                Direction = NatsDir.East;
            }
            else
            {
                Direction = NatsDir.West;
            }
        }

        private string getProperty(string str, string name)
        {
            int x = str.IndexOf("<" + name + ">");
            int y = str.IndexOf("</" + name + ">");
            return str.Substring(x + name.Length + 2, y - x - name.Length - 2);
        }

        private string NatsDirectionString()
        {
            if (Direction == NatsDir.East)
            {
                return "East";
            }
            else
            {
                return "West";
            }
        }

        public XElement ConvertToXml()
        {
            return new XElement("Content", new XElement[] {
                                                    new XElement("LastUpdated", LastUpdated),
                                                    new XElement("GeneralInfo", GeneralInfo),
                                                    new XElement("Direction", NatsDirectionString()),
                                                    new XElement("Message", Message)});
        }



        public List<NorthAtlanticTrack> ConvertToTracks()
        {
            char trkStartChar = (Direction == NatsDir.West ? 'A' : 'N');

            var tracks = new List<NorthAtlanticTrack>();

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

                tracks.Add(new NorthAtlanticTrack(Direction, (char)i, new List<string>(wp), new List<int>()));
            }

            return tracks;
        }

    }

}

