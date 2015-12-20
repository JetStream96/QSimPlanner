using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.RouteFinding.Data;

namespace QSP.RouteFinding.Containers
{
    public class WptSeachWrapper : ICoordinate, IEquatable<WptSeachWrapper>
    {
        public int Index;
        public double Lat { get; set; }
        public double Lon { get; set; }

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