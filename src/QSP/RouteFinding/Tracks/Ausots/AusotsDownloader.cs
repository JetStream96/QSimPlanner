using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsDownloader : IAusotsMessageProvider
    {
        private const string address =
            "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public AusotsMessage GetMessage()
        {
            Thread.Sleep(20000);//TODO:remove this
            using (var wc = new WebClient())
            {
                return new AusotsMessage(wc.DownloadString(address));
            }
        }

        public async Task<AusotsMessage> GetMessageAsync(CancellationToken token)
        {
            Thread.Sleep(20000);//TODO:remove this
            using (var wc = new WebClient())
            {
                token.Register(wc.CancelAsync);
                var str = await wc.DownloadStringTaskAsync(new Uri(address));
                return new AusotsMessage(str);
            }
        }
    }
}
