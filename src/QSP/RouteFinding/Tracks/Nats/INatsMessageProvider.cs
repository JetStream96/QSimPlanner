using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Nats
{
    public interface INatsMessageProvider
    {
        /// <exception cref="Exception"></exception>
        NatsMessage GetMessage();

        /// <exception cref="Exception"></exception>
        Task<NatsMessage> GetMessageAsync(CancellationToken token);
    }
}
