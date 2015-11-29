using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace QSP.RouteFinding.Containers
{

    public class ChangeTracker
    {
        private int _regionStart;
        private int _regionEnd;
        //index of wpt whose neighbor is added (to the end of neighbor list, of course)
        private List<int> _addedNeighbor;

        private ChangeCategory _category;

        public ChangeTracker(int regionStart, ChangeCategory category)
        {
            _regionStart = regionStart;
            _category = category;
            _addedNeighbor = new List<int>();
            _regionEnd = -1;
        }

        public int RegionStart
        {
            get { return _regionStart; }
        }

        public int RegionEnd
        {
            get { return _regionEnd; }
            set
            {
                if (_regionEnd < 0)
                {
                    _regionEnd = value;
                }
                else
                {
                    throw new InvalidOperationException("RegionEnd is already set and cannot be changed after set.");
                }
            }
        }


        public ReadOnlyCollection<int> AddedNeighbor
        {
            get { return _addedNeighbor.AsReadOnly(); }
        }

        public ChangeCategory Category
        {
            get { return _category; }
        }

        public void AddNeighborRecord(int indexWpt)
        {
            _addedNeighbor.Add(indexWpt);
        }

        public void SetRegionEnd(int index)
        {
            _regionEnd = index;
        }

    }

    public enum ChangeCategory
    {
        Normal,
        Nats,
        Pacots,
        Ausots
    }

}
