using System.Collections.Generic;
using QSP.LibraryExtension;

namespace QSP.RouteFinding.AirwayStructure
{
    public class TrackerItem
    {
        #region Fields

        private Stack<int> _addedWpt;
        private Stack<int> _addedNeighbor;
        private ChangeCategory _category;

        #endregion

        public TrackerItem(ChangeCategory category)
        {
            _addedWpt = new Stack<int>();
            _addedNeighbor = new Stack<int>();
            _category = category;
        }

        public ReadOnlyStack<int> AddedWaypoint
        {
            get { return _addedWpt.AsReadOnly(); }
        }

        public ReadOnlyStack<int> AddedNeighbor
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

        public void AddNeighborRecord(int edgeIndex)
        {
            _addedNeighbor.Push(edgeIndex);
        }
    }
}

