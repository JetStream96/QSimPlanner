using System;
using System.Collections.Generic;
using System.Xml.Linq;
namespace QSP.RouteFinding.Tracks.Nats
{

    public enum NATsDir
    {
        East,
        West
    }

    public class NATsMessage
    {

        public string LastUpdated { get; set; }
        public string GeneralInfo { get; set; }
        public NATsDir Direction { get; set; }
        public string Message { get; set; }

        public NATsMessage(string LastUpdated, string GeneralInfo, NATsDir Direction, string Message)
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

            LastUpdated = root.Element("LastUpdated").Value ;
            //getProperty(str, "LastUpdated")
            GeneralInfo = root.Element("GeneralInfo").Value;
            //getProperty(str, "GeneralInfo")
            Message =root.Element("Message").Value;
            //getProperty(str, "Message")

            string s = root.Element("Direction").Value;
            //getProperty(str, "Direction")
            if (s == "East")
            {
                Direction = NATsDir.East;
            }
            else
            {
                Direction = NATsDir.West;
            }

        }

        private string getProperty(string str, string name)
        {

            int x = 0;
            int y = 0;

            x = str.IndexOf("<" + name + ">");
            y = str.IndexOf("</" + name + ">");
            return str.Substring(x + name.Length + 2, y - x - name.Length - 2);

        }

        public XElement ConvertToXml()
        {

            string s = null;

            if ((Direction == NATsDir.East))
            {
                s = "East";
            }
            else
            {
                s = "West";
            }

            return new XElement("Content", new XElement[] {
                new XElement("LastUpdated", LastUpdated),
                new XElement("GeneralInfo", GeneralInfo),
                new XElement("Direction", s),
                new XElement("Message", Message)
            });

        }

        public List<NorthAtlanticTrack> ConvertToTracks()
        {

            int startAscii = 0;

            switch (Direction)
            {
                case NATsDir.West:
                    startAscii = 65;
                    break;
                case NATsDir.East:
                    startAscii = 78;
                    break;
            }

            List<NorthAtlanticTrack> tracks = new List<NorthAtlanticTrack>();


            for (int i = startAscii; i <= startAscii + 12; i++)
            {
                int j = Message.IndexOf("\n" + new string((char)i,1)  + " ");

                if (j == -1)
                {
                    continue;
                }

                int k = Message.IndexOf('\n', j + 3);

                string str = Message.Substring(j + 3, k - j - 3);
                string[] wp = str.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                NorthAtlanticTrack t = new NorthAtlanticTrack();
                t.Direction = Direction;
                t.Ident =(char)i;
                t.WptIdent.AddRange(wp);

                tracks.Add(t);

            }

            return tracks;
        }

    }

}

