using System;
using static QSP.RouteFinding.Constants;

namespace QSP.RouteFinding.Containers
{
    public class Neighbor
    {
        // This class is immutable.

        public string Airway { get; private set; }
        public double Distance { get; private set; }

        public Neighbor(string Airway, double Distance)
        {
            this.Airway = Airway;
            this.Distance = Distance;
        }

        /// <summary>
        /// Value comparison of two Neighbors.
        /// </summary>
        public bool Equals(Neighbor x)
        {
            return (Airway == x.Airway && Math.Abs(Distance - x.Distance) < LATLON_TOLERANCE);
        }

    }
}
