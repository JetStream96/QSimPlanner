using System;
using static QSP.AviationTools.Coordinates.Constants;

namespace QSP.RouteFinding.Containers
{
    // This class is immutable.
    public class Neighbor
    {
        public string Airway { get; private set; }
        public AirwayType AirwayType { get; private set; }
        public double Distance { get; private set; }

        public Neighbor(string Airway, AirwayType AirwayType, double Distance)
        {
            this.Airway = Airway;
            this.AirwayType = AirwayType;
            this.Distance = Distance;
        }

        /// <summary>
        /// Value comparison of two Neighbors.
        /// </summary>
        //public bool Equals(Neighbor x)
        //{
        //    return Airway == x.Airway &&
        //        AirwayType == AirwayType &&
        //        Math.Abs(Distance - x.Distance) < LatLonTolerance;
        //}
    }

    public enum AirwayType
    {
        Enroute,
        Terminal    // SID or STAR
    }
}
