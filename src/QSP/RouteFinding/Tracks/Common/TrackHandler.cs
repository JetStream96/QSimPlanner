using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    /// <summary>
    /// Provides some easy-to-use methods to manage tracks.
    /// </summary>
    public abstract class TrackHandler
    {
        /// <summary>
        /// Download and parse all track messages.
        /// </summary>
        public abstract void GetAllTracks();
        public abstract Task GetAllTracksAsync(CancellationToken token);

        /// <summary>
        /// Indicates whether GetAllTracks or GetAllTracksAsync has been called.
        /// </summary>
        public abstract bool StartedGettingTracks { get; }

        /// <summary>
        /// Add the parsed tracks to WaypointList, if not added already.
        /// </summary>
        public abstract void AddToWaypointList();

        /// <summary>
        /// Undo the actions of AddToWaypointList().
        /// </summary>
        public abstract void UndoEdit();     
    }
}
