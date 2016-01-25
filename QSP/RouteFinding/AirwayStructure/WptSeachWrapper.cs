using System;
using QSP.RouteFinding.Data;
using QSP.RouteFinding.Data.Interfaces;

namespace QSP.RouteFinding.AirwayStructure
{
    public class WptSeachWrapper : ICoordinate, IEquatable<WptSeachWrapper>
    {
        public int Index { get; private set; }
        public double Lat { get; private set; }
        public double Lon { get; private set; }

        public WptSeachWrapper(int index)
        {
            this.Index = index;
        }

        public WptSeachWrapper(int index, double lat, double lon)
        {
            this.Index = index;
            this.Lat = lat;
            this.Lon = lon;
        }

        public int GetHashCode(WptSeachWrapper obj)
        {
            return Index.GetHashCode();
        }

        public bool Equals(WptSeachWrapper other)
        {
            return (this.Index == other.Index);
        }
    }
}