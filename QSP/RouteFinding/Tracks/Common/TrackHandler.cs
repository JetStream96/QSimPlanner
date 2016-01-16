using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Tracks.Pacots;
using QSP.RouteFinding.Tracks.Interaction;
using static QSP.MathTools.Utilities;
using QSP.RouteFinding.AirwayStructure;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.Tracks.Common
{
    /// <summary>
    /// Provides some easy-to-use methods to manage tracks.
    /// </summary>
    public abstract class TrackHandler<T> where T : ITrack
    {
        /// <summary>
        /// Download and parse all track messages.
        /// </summary>
        public abstract void GetAllTracks();
        public abstract void GetAllTracksAsync();

        /// <summary>
        /// Add the downloaded tracks to WaypointList.
        /// </summary>
        public abstract void AddToWaypointList();        
    }
}
