using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public interface IPacotsDownloader
    {
        PacotsMessage Download();
        Task<PacotsMessage> DownloadAsync();
    }
}
