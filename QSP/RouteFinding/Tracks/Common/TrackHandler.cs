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
        public abstract Task GetAllTracksAsync();

        /// <summary>
        /// Add the downloaded tracks to WaypointList.
        /// </summary>
        public abstract void AddToWaypointList();        
    }
}
