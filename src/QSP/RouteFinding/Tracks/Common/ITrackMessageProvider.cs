using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrackMessageProvider
    {
        /// <exception cref="Exception"></exception>
        ITrackMessage GetMessage();

        /// <exception cref="Exception"></exception>
        Task<ITrackMessage> GetMessageAsync();
    }
}