using QSP.RouteFinding.Routes.TrackInUse;
using QSP.RouteFinding.Tracks.Common;
using System.Collections.Generic;

namespace QSP.RouteFinding.Communication
{
    public class RouteTrackCommunicator
    {
        private TrackInUseCollection tracksInUse;
        private List<TrackNodes> allNodes;

        public RouteTrackCommunicator(TrackInUseCollection tracksInUse)
        {
            this.tracksInUse = tracksInUse;
            allNodes = new List<TrackNodes>();
        }

        public void StageTrackData(TrackNodes nodes)
        {
            allNodes.Add(nodes);
        }

        public void PushAllData(TrackType type)
        {
            tracksInUse.UpdateTracks(allNodes, type);
            allNodes.Clear();
        }
    }
}
