using System.Collections.Generic;

namespace QSP.RouteFinding.Containers
{
    // This class is immutable.
    public class Neighbor
    {
        public string Airway { get; private set; }
        public double Distance { get; private set; }

        // This is null if the neighbor is a direct from one waypoint to
        // another. Otherwise, it includes all inner waypoints.
        // For example, if the neighbor is an oceanic track containing 
        // waypoints A, B, C, and D, the inner waypoints are B and C.
        // Also, in this case, the distance should be the totally distance
        // from A to D via B and C.
        // This should never has a Count of 0. Use null instead.
        // TODO: Do not use null.
        public IReadOnlyList<Waypoint> InnerWaypoints { get; private set; }

        public Neighbor(string Airway, double Distance,
            IReadOnlyList<Waypoint> InnerWaypoints = null)
        {
            this.Airway = Airway;
            this.Distance = Distance;
            this.InnerWaypoints = InnerWaypoints;
        }
    }
}
