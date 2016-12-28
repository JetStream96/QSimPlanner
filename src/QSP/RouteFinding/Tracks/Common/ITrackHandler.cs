using System.Threading.Tasks;
using QSP.RouteFinding.Tracks.Interaction;

namespace QSP.RouteFinding.Tracks.Common
{
    public interface ITrackHandler
    {
        ITrackMessage Message { get; }
        bool InWptList { get; }

        /// <summary>
        /// Download and parse all track messages.
        /// </summary>
        void GetAllTracks(StatusRecorder r);
        void GetAllTracks(ITrackMessageProvider provider, StatusRecorder r);
        Task GetAllTracksAsync(StatusRecorder r);

        /// <summary>
        /// Indicates whether GetAllTracks or GetAllTracksAsync has been called.
        /// </summary>
        bool StartedGettingTracks { get; }

        /// <summary>
        /// Add the parsed tracks to WaypointList, if not added already.
        /// </summary>
        void AddToWaypointList(StatusRecorder r);

        /// <summary>
        /// Undo the actions of AddToWaypointList().
        /// </summary>
        void UndoEdit();
    }
}