using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public interface IPacotsMessageProvider
    {
        /// <exception cref="Exception"></exception>
        PacotsMessage GetMessage();

        /// <exception cref="Exception"></exception>
        Task<PacotsMessage> GetMessageAsync(CancellationToken token);
    }
}
