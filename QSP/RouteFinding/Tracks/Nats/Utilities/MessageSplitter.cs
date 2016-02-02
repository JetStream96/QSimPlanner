using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Nats;
using QSP.LibraryExtension.StringParser;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Tracks.Nats.Utilities
{
    // Converts html source code to IndividualNatsMessage
    //
    public class MessageSplitter
    {
        private readonly static char[] specialChars =
            new char[] { (char)2, (char)3, (char)11 };

        private string html;

        private string timeUpdated;
        private string header;

        public MessageSplitter(string html)
        {
            this.html = html;
        }

        public List<IndividualNatsMessage> Split()
        {
            getGeneralInfo();

            var msgs = new List<IndividualNatsMessage>();
            var west = getWestboundTracks();
            var east = getEastboundTracks();

            if (west != null)
            {
                msgs.Add(west);
            }

            if (east != null)
            {
                msgs.Add(east);
            }
            return msgs;
        }

        private IndividualNatsMessage getWestboundTracks()
        {
            var sp = new StringParser(html);
            int index = html.IndexOf("EGGXZOZX");

            if (index < 0)
            {
                return null;
            }
            sp.CurrentIndex = index;

            sp.CurrentIndex = Math.Max(html.LastIndexOf('\n', sp.CurrentIndex), -1) + 1;
            string message = sp.ReadString(html.IndexOf("</td>", sp.CurrentIndex) - 1)
                               .RemoveHtmlTags()
                               .ReplaceAny(specialChars, "");

            return new IndividualNatsMessage(timeUpdated, header, NatsDirection.West, message);
        }

        private IndividualNatsMessage getEastboundTracks()
        {
            var sp = new StringParser(html);
            int index = html.IndexOf("CZQXZQZX");

            if (index < 0)
            {
                return null;
            }
            sp.CurrentIndex = index;

            sp.CurrentIndex = Math.Max(html.LastIndexOf('\n', sp.CurrentIndex), -1) + 1;
            string message = sp.ReadString(html.IndexOf("</td>", sp.CurrentIndex) - 1)
                               .RemoveHtmlTags()
                               .ReplaceAny(specialChars, "");

            return new IndividualNatsMessage(timeUpdated, header, NatsDirection.East, message);
        }

        private void getGeneralInfo()
        {
            var sp = new StringParser(html);
            sp.MoveToNextIndexOf("Last updated");
            timeUpdated = sp.ReadString(html.IndexOf("</", sp.CurrentIndex) - 1);

            sp.MoveToNextIndexOf("The following are active North Atlantic Tracks");
            header = sp.ReadString(html.IndexOf("</", sp.CurrentIndex) - 1);
        }
    }
}
