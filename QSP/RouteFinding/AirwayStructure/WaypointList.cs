using QSP.AviationTools;
using QSP.LibraryExtension.Graph;
using QSP.RouteFinding.Data;
using System;
using System.Collections.Generic;
using System.IO;
using static QSP.LibraryExtension.StringParser.Utilities;
using static QSP.MathTools.Utilities;
using static QSP.Utilities.ErrorLogger;
using QSP.RouteFinding.Tracks.Common;
using QSP.Core;
using QSP.RouteFinding.Containers;

namespace QSP.RouteFinding.AirwayStructure
{
    /// <summary>
    /// Representation of the airways and waypoints. 
    /// This is implemented with a hash table, so searching is by waypoint ident is O(1).
    /// This class is NOT thread safe.
    /// </summary>
    public class WaypointList
    {
        #region Fields

        private WaypointContainer _content;
        private ChangeTracker _changeTracker;
        private LatLonSearchUtility<WptSeachWrapper> _finder;

        #endregion

        public WaypointList()
        {
            _content = new WaypointContainer();
            _changeTracker = new ChangeTracker(this);
            _finder = new LatLonSearchUtility<WptSeachWrapper>(1, 5);
        }

        public TrackChangesOption CurrentlyTracked
        {
            get
            {
                return _changeTracker.CurrentlyTracked;
            }

            set
            {
                _changeTracker.CurrentlyTracked = value;
            }
        }

        /// <summary>
        /// Read all waypoints from ats.txt file.
        /// </summary>
        /// <param name="filepath">Path of ats.txt</param>
        /// <exception cref="LoadWaypointFileException"></exception>
        public void ReadAtsFromFile(string filepath)
        {
            new AtsFileLoader(this).ReadAtsFromFile(filepath);
        }

        /// <summary>
        /// Loads all waypoints in waypoints.txt.
        /// </summary>
        /// <param name="filepath">Location of waypoints.txt</param>
        /// <exception cref="LoadWaypointFileException"></exception>
        public void ReadFixesFromFile(string filepath)
        {
            CurrentlyTracked = TrackChangesOption.No;

            string[] allLines = File.ReadAllLines(filepath);

            foreach (var i in allLines)
            {
                try
                {
                    if (i.Length == 0 || i[0] == ' ')
                    {
                        continue;
                    }
                    int pos = 0;

                    string id = ReadString(i, ref pos, ',');
                    double lat = ParseDouble(i, ref pos, ',');
                    double lon = ParseDouble(i, ref pos, ',');

                    AddWpt(id, lat, lon);
                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    //TODO: Write to log file. Show to user, etc.
                    throw new LoadWaypointFileException("Failed to load waypoints.txt.", ex);
                }
            }
            CurrentlyTracked = TrackChangesOption.Yes;
        }

        private void addWptChanges(int index)
        {
            if (_changeTracker.CurrentlyTracked != TrackChangesOption.No)
            {
                _changeTracker.TrackWaypointAddition(index);
            }
            _finder.Add(new WptSeachWrapper(index, _content[index].Lat, _content[index].Lon));
        }

        public int AddWpt(string ID, double Lat, double Lon)
        {
            return AddWpt(new Waypoint(ID, Lat, Lon));
        }

        public int AddWpt(Waypoint item)
        {
            int index = _content.AddWpt(item);
            addWptChanges(index);
            return index;
        }

        public void AddNeighbor(int indexFrom, int indexTo, Neighbor item)
        {
            if (_changeTracker.CurrentlyTracked != TrackChangesOption.No)
            {
                _changeTracker.TrackNeighborAddition(_content.AddNeighbor(indexFrom, indexTo, item));
            }
            else
            {
                _content.AddNeighbor(indexFrom, indexTo, item);
            }
        }

        public Waypoint this[int index]
        {
            get
            {
                return _content[index];
            }
        }

        public LatLon LatLonAt(int index)
        {
            return this[index].LatLon;
        }

        public int EdgesFromCount(int index)
        {
            return _content.EdgesFromCount(index);
        }

        public int EdgesToCount(int index)
        {
            return _content.EdgesToCount(index);
        }

        public int Count
        {
            get { return _content.Count; }
        }

        /// <summary>
        /// The upper bound of indices of elements plus one. 
        /// </summary>
        public int MaxSize
        {
            get
            {
                return _content.MaxSize;
            }
        }

        /// <summary>
        /// Find the index of WptNeighbor by ident of a waypoint.
        /// </summary>
        public int FindByID(string ident)
        {
            return _content.FindByID(ident);
        }

        /// <summary>
        /// Find all WptNeighbors by ident of a waypoint.
        /// </summary> 
        public List<int> FindAllByID(string ident)
        {
            return _content.FindAllByID(ident);
        }

        /// <summary>
        /// Find the index of WptNeighbor matching the waypoint.
        /// </summary>
        public int FindByWaypoint(string ident, double lat, double lon)
        {
            return FindByWaypoint(new Waypoint(ident, lat, lon));
        }

        /// <summary>
        /// Find the index of WptNeighbor matching the waypoint. Returns -1 if no match is found.
        /// </summary>
        public int FindByWaypoint(Waypoint wpt)
        {
            return _content.FindByWaypoint(wpt);
        }

        /// <summary>
        /// Find all occurences of WptNeighbor matching the waypoint.
        /// </summary>
        public List<int> FindAllByWaypoint(Waypoint wpt)
        {
            return _content.FindAllByWaypoint(wpt);
        }

        public void Restore()
        {
            _changeTracker.RevertChanges(ChangeCategory.Normal);
        }

        public void DisableTrack(TrackType type)
        {
            switch (type)
            {
                case TrackType.Nats:
                    _changeTracker.RevertChanges(ChangeCategory.Nats);
                    break;

                case TrackType.Pacots:
                    _changeTracker.RevertChanges(ChangeCategory.Pacots);
                    break;

                case TrackType.Ausots:
                    _changeTracker.RevertChanges(ChangeCategory.Ausots);
                    break;

                default:
                    throw new EnumNotSupportedException();
            }
        }

        public List<WptSeachWrapper> Find(double lat, double lon, double distance)
        {
            return _finder.Find(lat, lon, distance);
        }

        public double Distance(int index1, int index2)
        {
            return GreatCircleDistance(this[index1].Lat, this[index1].Lon,
                                       this[index2].Lat, this[index2].Lon);
        }

        public void RemoveAt(int index)
        {
            _finder.Remove(new WptSeachWrapper(index, _content[index].Lat, _content[index].Lon));
            _content.RemoveAt(index);
        }

        public void RemoveNeighbor(int edgeIndex)
        {
            _content.RemoveNeighbor(edgeIndex);
        }

        public IEnumerable<int> EdgesFrom(int index)
        {
            return _content.EdgesFrom(index);
        }

        public IEnumerable<int> EdgesTo(int index)
        {
            return _content.EdgesTo(index);
        }

        public Edge<Neighbor> GetEdge(int edgeIndex)
        {
            return _content.GetEdge(edgeIndex);
        }

    }
}