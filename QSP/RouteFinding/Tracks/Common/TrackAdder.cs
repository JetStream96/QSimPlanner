using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.AirwayStructure;

namespace QSP.RouteFinding.Tracks.Common
{
    public class TrackAdder
    {
        private WaypointList wptList;

        public TrackAdder() : this(RouteFindingCore.WptList) { }

        public TrackAdder(WaypointList wptList)
        {
            this.wptList = wptList;
        }

        public void AddToWaypointList(TrackNodes nodes)
        {

        }
    }
}
