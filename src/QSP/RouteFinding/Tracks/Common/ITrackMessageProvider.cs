using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrackMessageProvider
    {
        /// <exception cref="Exception"></exception>
        ITrackMessageNew GetMessage();

        /// <exception cref="Exception"></exception>
        Task<ITrackMessageNew> GetMessageAsync(CancellationToken token);
    }
}