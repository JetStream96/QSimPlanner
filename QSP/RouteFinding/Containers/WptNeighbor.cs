using QSP.AviationTools;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.Containers
{

    public class WptNeighbor : Waypoint
    {
        protected List<Neighbor> _neighborList;

        public ReadOnlyCollection<Neighbor> Neighbors
        {
            // List(Of Neighbor)
            get { return _neighborList.AsReadOnly(); }
        }
        
        public WptNeighbor(string ID,double Lat,double Lon):base(ID,Lat,Lon)
        {
            _neighborList = new List<Neighbor>();
        }

        public WptNeighbor(Waypoint Waypoint) : this(Waypoint, new List<Neighbor>())
        {
            _neighborList = new List<Neighbor>();
        }

        public WptNeighbor(Waypoint Waypoint, List<Neighbor> neighborList) : base(Waypoint)
        {
            _neighborList = neighborList;
        }

        /// <summary>
        /// Copy the item to construct a new instance.
        /// </summary>
        public WptNeighbor(WptNeighbor item):this(new Waypoint(item), new List<Neighbor>(item.Neighbors))
        {
        }

        public void AddNeighbor(int index, string airway, double dis)
        {
            _neighborList.Add(new Neighbor(index, airway, dis));
        }
        
        /// <summary>
        /// Value comparison of two WptNeighbors.
        /// </summary>
        public bool Equals(WptNeighbor x)
        {
            if (base.Equals(x) && _neighborList.Count == x._neighborList.Count)
            {
                for (int i = 0; i < _neighborList.Count; i++)
                {
                    if (!_neighborList[i].Equals(x._neighborList[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
               
        public class WptNeighborEqualityComparer : IEqualityComparer<WptNeighbor>
        {
            public bool Equals(WptNeighbor x, WptNeighbor y)
            {
                return x.Equals(y);
            }

            public int GetHashCode(WptNeighbor obj)
            {
                int hash = obj.GetHashCode();

                foreach (var i in obj._neighborList)
                {
                    hash = hash ^ i.GetHashCode();
                }
                return hash;
            }
        }

    }

}
