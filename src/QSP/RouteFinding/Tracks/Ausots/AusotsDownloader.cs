using System.Net;

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
            return new AusotsMessage(new WebClient().DownloadString(address));
        }
    }
}
