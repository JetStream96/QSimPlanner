using System.Threading;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrackHandler
    {
        ITrackMessage RawData { get; }
        bool InWptList { get; }

        /// <summary>
        /// Download and parse all track messages.
        /// </summary>
        void GetAllTracks();
        void GetAllTracks(ITrackMessageProvider provider);
        Task GetAllTracksAsync();

        /// <summary>
        /// Indicates whether GetAllTracks or GetAllTracksAsync has been called.
        /// </summary>
        bool StartedGettingTracks { get; }

        /// <summary>
        /// Add the parsed tracks to WaypointList, if not added already.
        /// </summary>
        void AddToWaypointList();

        /// <summary>
        /// Undo the actions of AddToWaypointList().
        /// </summary>
        void UndoEdit();
    }
}