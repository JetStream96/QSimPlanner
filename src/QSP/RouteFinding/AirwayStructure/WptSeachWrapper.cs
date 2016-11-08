using QSP.RouteFinding.Data.Interfaces;
using System;

namespace QSP.RouteFinding.AirwayStructure
{
    public class WptSeachWrapper : ICoordinate, IEquatable<WptSeachWrapper>
    {
        public int Index { get; }
        public double Lat { get; }
        public double Lon { get; }
        
        public WptSeachWrapper(int Index, double Lat, double Lon)
        {
            this.Index = Index;
            this.Lat = Lat;
            this.Lon = Lon;
        }

        public override int GetHashCode()
        {
            return Index.GetHashCode();
        }

        public bool Equals(WptSeachWrapper other)
        {
            return other != null && Index == other.Index;
        }
    }
}