using System;
using System.Net;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;
using static QSP.LibraryExtension.WebRequests;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsDownloader : ITrackMessageProvider
    {
        private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public ITrackMessage GetMessage()
        {
            using (var wc = WebClientNoCache())
            {
                return new AusotsMessage(wc.DownloadString(address));
            }
        }

        public async Task<ITrackMessage> GetMessageAsync()
        {
            using (var wc = WebClientNoCache())
            {
                var str = await wc.DownloadStringTaskAsync(new Uri(address));
                return new AusotsMessage(str);
            }
        }
    }
}
