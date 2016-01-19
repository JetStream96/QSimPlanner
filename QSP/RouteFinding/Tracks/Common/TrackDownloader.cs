using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackDownloader
    {
        public abstract TrackMessage Download();
        public abstract Task<TrackMessage> DownloadAsync();
    }
}
