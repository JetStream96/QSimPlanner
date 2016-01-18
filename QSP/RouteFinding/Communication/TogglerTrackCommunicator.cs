using System.Collections.Generic;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Tracks.Common;

namespace QSP.RouteFinding.Communication
{
    public class TogglerTrackCommunicator
    {
        private RouteToggler toggler;
        private List<TrackNodes> allNodes;

        public TogglerTrackCommunicator(RouteToggler toggler)
        {
            this.toggler = toggler;
            allNodes = new List<TrackNodes>();
        }

        public void StageTrackData(TrackNodes nodes)
        {
            allNodes.Add(nodes);
        }

        public void PushAllData(TrackType type)
        {
            toggler.UpdateTracks(allNodes, type);
        }
    }
}
