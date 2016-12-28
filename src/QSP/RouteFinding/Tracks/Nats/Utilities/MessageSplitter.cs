using QSP.LibraryExtension;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QSP.RouteFinding.Tracks.Nats.Utilities
{
    // Converts html source code to IndividualNatsMessage
    //
    public class MessageSplitter
    {
        private string html;
        private string timeUpdated;
        private string header;

        public MessageSplitter(string html)
        {
            this.html = html.ToValidXmlString();
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
