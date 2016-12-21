using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Ausots
{
    public interface IAusotsMessageProvider
    {
        /// <exception cref="TrackParseException"></exception>
        AusotsMessage GetMessage();

        /// <exception cref="TrackParseException"></exception>
        Task<AusotsMessage> GetMessageAsync(CancellationToken token);
    }
}
