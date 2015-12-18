using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.Containers
{
    public class TrackerItem
    {
        public struct NeighborRecord
        {
            public int index; // The index of waypoint
            public Neighbor neighbor; // The instance of neighbor

            public NeighborRecord(int index, Neighbor neighbor)
            {
                this.index = index;
                this.neighbor = neighbor;
            }
        }

        #region Fields

        private Stack<int> _addedWpt;
        private Stack<NeighborRecord> _addedNeighbor;
        private ChangeCategory _category;

        #endregion

        public TrackerItem(ChangeCategory category)
        {
            _addedWpt = new Stack<int>();
            _addedNeighbor = new Stack<NeighborRecord>();
        }

        public ReadOnlyStack<int> AddedWaypoint
        {
            get { return _addedWpt.AsReadOnly(); }
        }

        public ReadOnlyStack<NeighborRecord> AddedNeighbor
        {
            get { return _addedNeighbor.AsReadOnly(); }
        }

        public ChangeCategory Category
        {
            get { return _category; }
        }

        public void AddWaypointRecord(int index)
        {
            _addedWpt.Push(index);
        }

        public void AddNeighborRecord(int indexWpt, Neighbor neighbor)
        {
            _addedNeighbor.Push(new NeighborRecord(indexWpt, neighbor));
        }
    }
}

