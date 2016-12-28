using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    public class TrackMessageProvider : ITrackMessageProvider
    {
        private ITrackMessage msg;

        public TrackMessageProvider(ITrackMessage msg)
        {
            this.msg = msg;
        }

        public ITrackMessage GetMessage() => msg;

        public Task<ITrackMessage> GetMessageAsync() => Task.FromResult(msg);
    }
}