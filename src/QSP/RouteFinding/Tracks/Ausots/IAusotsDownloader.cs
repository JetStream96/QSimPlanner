using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public interface IAusotsDownloader
    {
        AusotsMessage Download();
        Task<AusotsMessage> DownloadAsync();
    }
}
