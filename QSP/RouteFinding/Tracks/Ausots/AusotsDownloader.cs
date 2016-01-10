using System.Net;
using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public class AusotsDownloader : TrackDownloader<AusTrack>
    {
        private const string address = "https://www.airservicesaustralia.com/flextracks/text.asp?ver=1";

        public override TrackRawData<AusTrack> Download()
        {
            return new AusotsRawData(new WebClient().DownloadString(address));
        }

        public async override Task<TrackRawData<AusTrack>> DownloadAsync()
        {
            return await Task.Run(() => Download());
        }
    }
}
