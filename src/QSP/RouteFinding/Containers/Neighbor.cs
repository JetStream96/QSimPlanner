using System.Collections.Generic;

namespace QSP.RouteFinding.Containers
{
    // This class is immutable.
    public class Neighbor
    {
        private static readonly Waypoint[] EmptyInnerWaypoints = { };

        public string Airway { get; private set; }
        public double Distance { get; private set; }

        /// <summary>
        /// This has a Count of 0 if the neighbor is a direct from one 
        /// waypoint to another. Otherwise, it includes all inner waypoints.
        /// For example, if the neighbor is an oceanic track containing 
        /// waypoints A, B, C, and D, the inner waypoints are B and C.
        /// Also, in this case, the distance should be the total distance
        /// from A to D via B and C.
        /// </summary>
        public IReadOnlyList<Waypoint> InnerWaypoints { get; private set; }
        public NeighborType Type { get; private set; }

        public Neighbor(string Airway, double Distance)
            : this(Airway, Distance, EmptyInnerWaypoints, NeighborType.Enroute)
        { }

        public Neighbor(string Airway, double Distance,
            IReadOnlyList<Waypoint> InnerWaypoints, NeighborType Type)
        {
            this.Airway = Airway;
            this.Distance = Distance;
            this.InnerWaypoints = InnerWaypoints;
            this.Type = Type;
        }
    }

    public enum NeighborType
    {
        Terminal,
        Track,
        Enroute
    }
}
