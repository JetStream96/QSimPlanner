using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Nats
{
    public sealed class NatsDownloader : ITrackMessageProvider, IDisposable
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

        // Given the existing messages, returns the URLs of the missing NAT message(s).
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

        /// <summary>
        /// Downloads the track message.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public ITrackMessage GetMessage()
        {
            var natMsg = DownloadFromNotam();
            var htmls = AdditionalDownloads(natMsg)
                .Select(i => client.DownloadString(i));
            var additional = htmls.Select(xml => new IndividualNatsMessage(XDocument.Parse(xml)));

            natMsg.AddRange(additional);
            return CreateMessage(natMsg);
        }

        public async Task<ITrackMessage> GetMessageAsync()
        {
            var natMsg = await DownloadFromNotamAsync();
            var tasks = AdditionalDownloads(natMsg)
                .Select(i => client.DownloadStringTaskAsync(i));
            var htmls = await Task.WhenAll(tasks);
            var additional = htmls.Select(xml => new IndividualNatsMessage(XDocument.Parse(xml)));

            natMsg.AddRange(additional);
            return CreateMessage(natMsg);
        }

        private static NatsMessage CreateMessage(List<IndividualNatsMessage> msgs)
        {
            int westIndex = msgs[0].Direction == NatsDirection.West ? 0 : 1;
            int eastIndex = 1 - westIndex;
            return new NatsMessage(msgs[westIndex], msgs[eastIndex]);
        }

        public void Dispose()
        {
            client.Dispose();
        }

        ~NatsDownloader()
        {
            client.Dispose();
        }
    }
}
