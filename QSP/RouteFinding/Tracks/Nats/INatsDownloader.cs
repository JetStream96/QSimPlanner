using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Nats
{
    public interface INatsDownloader
    {
        NatsMessage Download();
        Task<NatsMessage> DownloadAsync();
    }
}
