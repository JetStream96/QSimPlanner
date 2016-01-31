using System.Collections.Generic;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.AirwayStructure
{
    public class TrackerItem
    {
        #region Fields
        // TODO: maybe use List instead.
        private Stack<int> _addedWpt;
        private Stack<int> _addedNeighbor;

        #endregion

        public TrackerItem()
        {
            _addedWpt = new Stack<int>();
            _addedNeighbor = new Stack<int>();
        }

        public ReadOnlyStack<int> AddedWaypoint
        {
            get { return _addedWpt.AsReadOnly(); }
        }

        public ReadOnlyStack<int> AddedNeighbor
        {
            get { return _addedNeighbor.AsReadOnly(); }
        }
        
        public void AddWaypointRecord(int index)
        {
            _addedWpt.Push(index);
        }

        public void AddNeighborRecord(int edgeIndex)
        {
            _addedNeighbor.Push(edgeIndex);
        }
    }
}

