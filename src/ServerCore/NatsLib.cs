using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using CommonLibrary.LibraryExtension;

namespace ServerCore
{
    // Classes in this file is copied from QSP project.
    public enum NatsDirection
    {
        East = 0,
        West = 1
    }

    public enum TrackType
    {
        Nats = 0,
        Pacots = 1,
        Ausots = 2
    }

    public class NatsMessage 
    {
        public IndividualNatsMessage WestMessage { get; private set; }
        public IndividualNatsMessage EastMessage { get; private set; }

        public NatsMessage(IndividualNatsMessage WestMessage,
            IndividualNatsMessage EastMessage)
        {
            this.WestMessage = WestMessage;
            this.EastMessage = EastMessage;
        }

        public NatsMessage(XDocument doc)
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

        public XDocument ToXml()
        {
            var west = WestMessage.ConvertToXml().Root;
            var east = EastMessage.ConvertToXml().Root;

            return new XDocument(
                new XElement("Content",
                    new XElement("TrackSystem", "NATs"),
                    new XElement[] {
                        new XElement("Westbound", west.Elements()),
                        new XElement("Eastbound", east.Elements())}));
        }

        // lastUpdated Should include format like '2017/06/07 15:38 GMT'
        public static (bool success, DateTime date) ParseDate(string lastUpdated)
        {
            try
            {
                var re = new Regex(@"(\d{4})\D+(\d{1,2})\D+(\d{1,2})\D+(\d{1,2})\D+(\d{1,2})");
                var match = re.Match(lastUpdated);
                var nums = match.Groups
                    .Cast<Group>()
                    .Skip(1)
                    .Take(5)
                    .Select(g => int.Parse(g.Value))
                    .ToList();
                return (true, new DateTime(nums[0], nums[1], nums[2], nums[3], nums[4],
                    0, 0, DateTimeKind.Utc));
            }
            catch
            {
                return (false, new DateTime());
            }
        }
    }

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

    public class MessageSplitter
    {
        private string html;
        private string timeUpdated;
        private string header;

        public MessageSplitter(string html)
        {
            this.html = html.RemoveIllegalXmlChar();
        }

        public List<IndividualNatsMessage> Split()
        {
            GetGeneralInfo();

            var msgs = new List<IndividualNatsMessage>();
            var west = GetWestboundTracks();
            var east = GetEastboundTracks();

            if (west != null) msgs.Add(west);
            if (east != null) msgs.Add(east);

            return msgs;
        }

        private IndividualNatsMessage GetWestboundTracks()
        {
            var match = Regex.Match(html, @"\n([^\n]*?EGGXZOZX.*?)</td>", RegexOptions.Singleline);

            if (!match.Success) return null;
            var message = match.Groups[1].Value.RemoveHtmlTags();

            return new IndividualNatsMessage(timeUpdated, header, NatsDirection.West, message);
        }

        private IndividualNatsMessage GetEastboundTracks()
        {
            var match = Regex.Match(html, @"\n([^\n]*?CZQXZQZX.*?)</td>", RegexOptions.Singleline);

            if (!match.Success) return null;
            var message = match.Groups[1].Value.RemoveHtmlTags();

            return new IndividualNatsMessage(timeUpdated, header, NatsDirection.East, message);
        }

        private void GetGeneralInfo()
        {
            var matchTime = Regex.Match(html, @"(Last updated.*?)</");
            timeUpdated = matchTime.Groups[1].Value;

            var matchHeader = Regex.Match(html,
                @"(The following are active North Atlantic Tracks.*?)</");
            header = matchHeader.Groups[1].Value;
        }
    }
}

