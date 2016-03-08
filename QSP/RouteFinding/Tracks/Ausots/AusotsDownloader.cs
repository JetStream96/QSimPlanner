using System;
using System.Net;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsDownloader : IAusotsDownloader
    {
        private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public AusotsMessage Download()
        {
            return new AusotsMessage(new WebClient().DownloadString(address));
        }

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<AusotsMessage> DownloadAsync()
        {
            return await Task.Run(() => Download());
        }
    }
}
