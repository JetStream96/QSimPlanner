using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using static QSP.LibraryExtension.Strings;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsDownloader : INatsDownloader
    {
        public const string natsUrl = "https://www.notams.faa.gov/common/nat.html?";
        private const string natsWest = "http://qsimplan.somee.com/nats/Westbound.xml";
        private const string natsEast = "http://qsimplan.somee.com/nats/Eastbound.xml";

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public static List<IndividualNatsMessage> DownloadFromWeb(string url)
        {
            //the list contains either 1 or 2 item(s)
            var result = new List<IndividualNatsMessage>();
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
                string time_updated = StringStartEndWith(htmlStr, "Last updated", "</i>", CutStringOptions.PreserveStart);
                string general_info = StringStartEndWith(htmlStr, "The following are active North Atlantic Tracks", "</th>", CutStringOptions.PreserveStart);
                if (htmlStr.IndexOf("EGGXZOZX") >= 0)
                {
                    string msg = CutString2(htmlStr, "EGGXZOZX", "</td>", false);
                    msg = ReplaceString(msg, new string[] {
                    "</font>",
                    "<font color=\"#000099\">",
                    new string((char)2,1),new string((char)3,1),new string((char)11,1)}, "");

                    result.Add(new IndividualNatsMessage(time_updated, general_info, NatsDirection.West, msg));
                }

                if (htmlStr.IndexOf("CZQXZQZX") >= 0)
                {
                    string msg = CutString2(htmlStr, "CZQXZQZX", "</td>", false);
                    msg = ReplaceString(msg, new string[]{
                    "</font>",
                    "<font color=\"#000099\">",
                     new string((char)2,1),new string((char)3,1),new string((char)11,1) }, "");

                    result.Add(new IndividualNatsMessage(time_updated, general_info, NatsDirection.East, msg));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new TrackParseException("", ex);
            }
        }

        //Repeated downloads is okay and will not cause any problem.
        private static List<IndividualNatsMessage> DownloadNatsMsg()
        {
            var natMsg = DownloadFromWeb(natsUrl);

            if (natMsg.Count == 1)
            {
                string downloadAdditional = null;

                if (natMsg[0].Direction == NatsDirection.East)
                {
                    downloadAdditional = natsWest;
                }
                else
                {
                    downloadAdditional = natsEast;
                }

                using (WebClient wc = new WebClient())
                {
                    natMsg.Add(new IndividualNatsMessage(XDocument.Parse(wc.DownloadString(downloadAdditional))));
                }
            }
            return natMsg;
        }

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public NatsMessage Download()
        {
            var msgs = DownloadNatsMsg();
            int westIndex = msgs[0].Direction == NatsDirection.West ? 0 : 1;
            int eastIndex = 1 - westIndex;
            return new NatsMessage(msgs[westIndex], msgs[eastIndex]);
        }

        /// <exception cref="TrackDownloadException"></exception>
        /// <exception cref="TrackParseException"></exception>
        public Task<NatsMessage> DownloadAsync()
        {
            return Task.Factory.StartNew(Download);
        }
    }
}
