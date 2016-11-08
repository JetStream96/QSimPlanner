using System.Collections.Generic;

namespace QSP.RouteFinding.AirwayStructure
{
    public class TrackerItem
    {
        private List<int> _addedWpt;
        private List<int> _addedNeighbor;

        public TrackerItem()
        {
            _addedWpt = new List<int>();
            _addedNeighbor = new List<int>();
        }

        public IEnumerable<int> AddedWaypoint => _addedWpt; 
        public IEnumerable<int> AddedNeighbor => _addedNeighbor; 
        
        public void AddWaypointRecord(int index)
        {
            _addedWpt.Add(index);
        }

        public void AddNeighborRecord(int edgeIndex)
        {
            _addedNeighbor.Add(edgeIndex);
        }
    }
}

