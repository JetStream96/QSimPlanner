using System.Collections.Generic;
using System.Linq;
using CommonLibrary.LibraryExtension;

namespace QSP.RouteFinding.Containers
{
    // This class is immutable.
    public class Neighbor
    {
        private static readonly Waypoint[] EmptyInnerWaypoints = { };

        public string Airway { get; }
        public double Distance { get; }

        /// <summary>
        /// This has a Count of 0 if the neighbor is a direct from one 
        /// waypoint to another. Otherwise, it includes all inner waypoints.
        /// For example, if the neighbor is an oceanic track containing 
        /// waypoints A, B, C, and D, the inner waypoints are B and C.
        /// Also, in this case, the distance should be the total distance
        /// from A to D via B and C.
        /// </summary>
        public IReadOnlyList<Waypoint> InnerWaypoints { get; }

        // The type of InnerWaypoints, if it's not empty. Otherwise, it should
        // be None.
        public InnerWaypointsType Type { get; }

        public Neighbor(string Airway, double Distance)
            : this(Airway, Distance, EmptyInnerWaypoints, InnerWaypointsType.None)
        { }

        public Neighbor(string Airway, double Distance,
            IReadOnlyList<Waypoint> InnerWaypoints, InnerWaypointsType Type)
        {
            this.Airway = Airway;
            this.Distance = Distance;
            this.InnerWaypoints = InnerWaypoints;
            this.Type = Type;
        }

        public bool Equals(Neighbor other)
        {
            return other != null &&
                Airway == other.Airway &&
                Distance == other.Distance &&
                Type == other.Type &&
                InnerWaypoints.SequenceEqual(other.InnerWaypoints);
        }

        public override int GetHashCode()
        {
            return Airway.GetHashCode() ^
                Distance.GetHashCode() ^
                Type.GetHashCode() ^
                InnerWaypoints.HashCodeByElem();
        }
    }

    public enum InnerWaypointsType
    {
        Terminal,
        Track,
        None
    }
}
