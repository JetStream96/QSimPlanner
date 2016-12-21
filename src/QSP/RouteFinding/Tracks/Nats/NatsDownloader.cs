using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Tracks.Nats
{
    public class NatsDownloader : INatsMessageProvider
    {
        public static readonly string natsUrl = "https://www.notams.faa.gov/common/nat.html?";
        private static readonly string natsWest = "http://qsimplan.somee.com/nats/Westbound.xml";
        private static readonly string natsEast = "http://qsimplan.somee.com/nats/Eastbound.xml";

        private WebClient client = new WebClient();

        public List<IndividualNatsMessage> DownloadFromNotam()
        {
            // The list should normally contains either 1 or 2 item(s).
            // But it's ok if it contains no item.

            return Split(GetNotamHtml());
        }

        public async Task<List<IndividualNatsMessage>> DownloadFromNotamAsync()
        {
            var htmlStr = await GetNotamHtmlAsync();
            return Split(htmlStr);
        }

        private static List<IndividualNatsMessage> Split(string html)
        {
            return new Utilities.MessageSplitter(html).Split();
        }

        private string GetNotamHtml() => client.DownloadString(natsUrl);

        private async Task<string> GetNotamHtmlAsync()
        {
            return await client.DownloadStringTaskAsync(natsUrl);
        }

        private static string[] AdditionalDownloads(List<IndividualNatsMessage> msgs)
        {
            string[] addresses = { natsWest, natsEast };

            if (msgs.Count == 0) return addresses;
            if (msgs.Count == 2) return new string[0];

            // Count is 1.
            string additional = msgs[0].Direction == NatsDirection.East
                ? natsWest
                : natsEast;

            return new[] { additional };
        }
        
        private List<IndividualNatsMessage> DownloadNatsMsg()
        {
            var natMsg = DownloadFromNotam();
            var additional = AdditionalDownloads(natMsg)
                .Select(i => client.DownloadString(i))
                .Select(xml => new IndividualNatsMessage(XDocument.Parse(xml)));

            natMsg.AddRange(additional);
            return natMsg;
        }

        private async Task<List<IndividualNatsMessage>> DownloadNatsMsgAsync()
        {
            var natMsg = await DownloadFromNotamAsync();
            var additional = AdditionalDownloads(natMsg)
                .Select(async i => await client.DownloadStringTaskAsync(i))
                .Select(async xml => await new IndividualNatsMessage(XDocument.Parse(xml)));

            natMsg.AddRange(additional);
            return natMsg;
        }

        /// <summary>
        /// Downloads the track message.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public NatsMessage GetMessage()
        {
            var msgs = DownloadNatsMsg();
            int westIndex = msgs[0].Direction == NatsDirection.West ? 0 : 1;
            int eastIndex = 1 - westIndex;
            return new NatsMessage(msgs[westIndex], msgs[eastIndex]);
        }

        public Task<NatsMessage> GetMessageAsync(CancellationToken token)
        {

        }

        ~NatsDownloader()
        {
            client.Dispose();
        }
    }
}
