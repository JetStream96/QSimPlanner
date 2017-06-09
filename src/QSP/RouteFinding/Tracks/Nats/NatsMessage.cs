using System;
using QSP.RouteFinding.Tracks.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Linq;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsMessage : ITrackMessage
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
                    new XElement("TrackSystem", TrackType.Nats.TrackString()),
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
}
