using System.Net;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsDownloader : TrackDownloader
    {
        private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public override TrackMessage Download()
        {
            return new AusotsMessage(new WebClient().DownloadString(address));
        }

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async override Task<TrackMessage> DownloadAsync()
        {
            return await Task.Run(() => Download());
        }
    }
}
