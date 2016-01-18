using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    public abstract class TrackDownloader<T> where T : ITrack
    {
        public abstract TrackMessage<T> Download();
        public abstract Task<TrackMessage<T>> DownloadAsync();
    }
}
