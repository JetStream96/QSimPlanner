using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsDownloader : ITrackMessageProvider
    {
        private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public ITrackMessage GetMessage()
        {
            Thread.Sleep(20000);//TODO:remove this
            using (var wc = new WebClient())
            {
                return new AusotsMessage(wc.DownloadString(address));
            }
        }

        public async Task<ITrackMessage> GetMessageAsync(CancellationToken token)
        {
            await Task.Factory.StartNew(() => Thread.Sleep(20000));//TODO:remove this
            using (var wc = new WebClient())
            {
                token.Register(wc.CancelAsync);
                var str = await wc.DownloadStringTaskAsync(new Uri(address));
                return new AusotsMessage(str);
            }
        }
    }
}
