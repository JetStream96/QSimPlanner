using System.Net;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;
using System;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsDownloader : TrackDownloader<AusTrack>
    {
        private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public override TrackRawData<AusTrack> Download()
        {
            return new AusotsRawData(new WebClient().DownloadString(address));
        }

        /// <exception cref="WebException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public async override Task<TrackRawData<AusTrack>> DownloadAsync()
        {
            return await Task.Run(() => Download());
        }
    }
}
