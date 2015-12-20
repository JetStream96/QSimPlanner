using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.RouteFinding.Containers
{
    public struct IndexDistancePair
    {
        public int Index;
        public double Distance;

        public IndexDistancePair(int Index, double Distance)
        {
            this.Index = Index;
            this.Distance = Distance;
        }
    }
}
