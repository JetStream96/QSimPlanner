using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsDownloader : INatsMessageProvider
    {
        public const string natsUrl = "https://www.notams.faa.gov/common/nat.html?";
        private const string natsWest = "http://qsimplan.somee.com/nats/Westbound.xml";
        private const string natsEast = "http://qsimplan.somee.com/nats/Eastbound.xml";

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public static List<IndividualNatsMessage> DownloadFromWeb(string url)
        {
            // The list should normally contains either 1 or 2 item(s).
            // But it's ok if it contains no item.

            string htmlStr;

            try
            {
                using (WebClient wc = new WebClient())
                {
                    htmlStr = wc.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                throw new TrackDownloadException("", ex);
            }

            try
            {
                return new Utilities.MessageSplitter(htmlStr).Split();
            }
            catch (Exception ex)
            {
                throw new TrackParseException("", ex);
            }
        }

        private static List<IndividualNatsMessage> DownloadNatsMsg()
        {
            var natMsg = DownloadFromWeb(natsUrl);

            if (natMsg.Count == 0)
            {
                using (WebClient wc = new WebClient())
                {
                    natMsg.Add(new IndividualNatsMessage(
                         XDocument.Parse(wc.DownloadString(natsWest))));

                    natMsg.Add(new IndividualNatsMessage(
                         XDocument.Parse(wc.DownloadString(natsEast))));
                }
            }
            else if (natMsg.Count == 1)
            {
                string downloadAdditional =
                    natMsg[0].Direction == NatsDirection.East
                    ? natsWest
                    : natsEast;

                using (WebClient wc = new WebClient())
                {
                    var xdoc = XDocument.Parse(
                        wc.DownloadString(downloadAdditional));

                    natMsg.Add(new IndividualNatsMessage(xdoc));
                }
            }
            return natMsg;
        }

        /// <summary>
        /// Downloads the track message.
        /// </summary>
        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public NatsMessage GetMessage()
        {
            var msgs = DownloadNatsMsg();
            int westIndex = msgs[0].Direction == NatsDirection.West ? 0 : 1;
            int eastIndex = 1 - westIndex;
            return new NatsMessage(msgs[westIndex], msgs[eastIndex]);
        }
    }
}
