using System.Net;
using System.Threading;

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
            Thread.Sleep(10000);//TODO:remove this
            return new AusotsMessage(new WebClient().DownloadString(address));
        }
    }
}
