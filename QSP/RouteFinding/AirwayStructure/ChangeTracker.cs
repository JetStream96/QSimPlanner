using System.Collections.Generic;

namespace QSP.RouteFinding.AirwayStructure
{
    /// <summary>
    /// Track all changes in WaypointList class. Provides methods to revert changes.
    /// </summary>
    public class ChangeTracker
    {
        private WaypointList _content;
        private List<TrackerItem> _trackerCollection;
        private TrackerItem _currentTracker;
        private TrackChangesOption _currentlyTracked;  // The category of change currently tracking.

        public ChangeTracker(WaypointList content)
        {
            _content = content;
            _trackerCollection = new List<TrackerItem>();
            createNewSession(TrackChangesOption.Yes);
        }

        public TrackChangesOption CurrentlyTracked
        {
            get { return _currentlyTracked; }

            set
            {
                // Repeatedly setting the same value.
                if (_currentlyTracked == value)
                {
                    return;
                }

                endCurrentSession();
                createNewSession(value);
            }
        }

        private void createNewSession(TrackChangesOption value)
        {
            if (value != TrackChangesOption.No)
            {
                _currentTracker = new TrackerItem(Utilities.ToChangeCategory(value));
            }
            _currentlyTracked = value;
        }

        private void endCurrentSession()
        {
            if (_currentTracker != null)
            {
                _trackerCollection.Add(_currentTracker);
                _currentTracker = null;
            }
        }

        public void TrackWaypointAddition(int index)
        {
            _currentTracker.AddWaypointRecord(index);
        }

        public void TrackNeighborAddition(int edgeIndex)
        {
            _currentTracker.AddNeighborRecord(edgeIndex);
        }

        public void RevertChanges(ChangeCategory para)
        {
            // There's no need to check the value of _currentlyTracked.
            if (_trackerCollection.Count > 0)
            {
                for (int i = _trackerCollection.Count - 1; i >= 0; i--)
                {
                    if (_trackerCollection[i].Category == para)
                    {
                        var item = _trackerCollection[i];

                        //remove neighbors first
                        foreach (var j in item.AddedNeighbor)
                        {
                            _content.RemoveNeighbor(j);
                        }

                        // Remove all wpts.
                        foreach (var k in item.AddedWaypoint)
                        {
                            _content.RemoveAt(k);
                        }

                        // Remove TrackerItem from collection.
                        _trackerCollection.RemoveAt(i);
                    }
                }
            }

            // Set to track changes automatically.
            CurrentlyTracked = TrackChangesOption.Yes;
        }
    }

}
