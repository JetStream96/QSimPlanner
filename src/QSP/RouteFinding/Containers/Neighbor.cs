using System.Collections.Generic;

namespace QSP.RouteFinding.Containers
{
    // This class is immutable.
    public class Neighbor
    {
        public string Airway { get; private set; }
        public double Distance { get; private set; }
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
