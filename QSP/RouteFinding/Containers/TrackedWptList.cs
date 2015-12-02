using QSP.AviationTools;
using QSP.LibraryExtension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.MathTools.MathTools;
using static QSP.Utilities.ErrorLogger;

namespace QSP.RouteFinding.Containers
{
    /// <summary>
    /// Representation of the airways and waypoints. 
    /// This is implemented with a hash table, so searching is by waypoint ident is O(1).
    /// This class is NOT thread safe.
    /// </summary>
    public class TrackedWptList
    {

        #region "Fields"
        
        private HashMap<string, int> searchHelper;
        private List<WptNeighbor> content;

        //indicates how many other waypoints have this wpt as neighbor
        private List<int> numNodeFrom;

        private List<ChangeTracker> trackerCollection;
        private ChangeTracker currentTracker;

        private TrackChangesOption _trackChanges;

        //Used to add neighbor to a WptNeighbor. Do not pass this object outside this class.
        private static readonly object token = new object();

        #endregion
        
        public TrackedWptList()
        {
            _trackChanges = TrackChangesOption.No;
            content = new List<WptNeighbor>();
            searchHelper = new HashMap<string, int>();
            numNodeFrom = new List<int>();
            trackerCollection = new List<ChangeTracker>();
            currentTracker = null;
        }

        public TrackChangesOption TrackChanges
        {
            get { return _trackChanges; }

            set
            {
                //First, stop current tracking session.
                endCurrentSession();

                switch (value)
                {
                    case TrackChangesOption.Yes:
                        currentTracker = new ChangeTracker(Count, ChangeCategory.Normal);
                        break;

                    case TrackChangesOption.AddingNATs:
                        currentTracker = new ChangeTracker(Count, ChangeCategory.Nats);
                        break;

                    case TrackChangesOption.AddingPacots:
                        currentTracker = new ChangeTracker(Count, ChangeCategory.Pacots);
                        break;

                    case TrackChangesOption.AddingAusots:
                        currentTracker = new ChangeTracker(Count, ChangeCategory.Ausots);
                        break;

                }
                _trackChanges = value;
            }
        }
        
        private void endCurrentSession()
        {
            if (currentTracker != null)
            {
                currentTracker.RegionEnd = Count - 1;
                trackerCollection.Add(currentTracker);
                currentTracker = null;
            }
        }
        
        public void ReadAtsFromFile(string filepath)
        {
            try
            {
                TrackChanges = TrackChangesOption.No;

                //add error handling
                string[] allLines = File.ReadAllLines(filepath);
                string[] line = null;
                string currentAirway = "";

                foreach (var i in allLines)
                {
                    if (i == "")
                    {
                        continue;
                    }

                    line = i.Split(',');

                    if (line.Length == 3)
                    {
                        //this line is an airway identifier
                        currentAirway = line[1];
                    }
                    else if (line.Length >= 10)
                    {
                        //this line is waypoint

                        Waypoint secondWpt = new Waypoint((line[4]), Convert.ToDouble(line[5]), Convert.ToDouble(line[6]));
                        Neighbor secondWptAsNeighbor = null;
                        Waypoint firstWpt = new Waypoint(line[1], Convert.ToDouble(line[2]), Convert.ToDouble(line[3]));

                        int index1 = FindByWaypoint(firstWpt);
                        int index2 = FindByWaypoint(secondWpt);

                        //add second waypoint as required
                        if (index2 >= 0)
                        {
                            secondWptAsNeighbor = new Neighbor(index2, currentAirway, Convert.ToDouble(line[9]));
                        }
                        else
                        {
                            secondWptAsNeighbor = new Neighbor(this.Count, currentAirway, Convert.ToDouble(line[9]));
                            AddWpt(new WptNeighbor(secondWpt));
                        }

                        //add first waypoint as required
                        if (index1 >= 0)
                        {
                            AddNeighbor(index1, secondWptAsNeighbor);
                        }
                        else
                        {
                            AddWpt(new WptNeighbor(firstWpt));
                            AddNeighbor(this.Count - 1, secondWptAsNeighbor);
                        }
                    }
                }

                TrackChanges = TrackChangesOption.Yes;

            }
            catch (Exception ex)
            {
                WriteToLog(ex);
                throw;  //TODO: show to the user
            }
        }

        /// <summary>
        /// Loads all waypoints in waypoints.txt.
        /// </summary>
        /// <param name="filepath">Location of waypoints.txt</param>
        public void ReadFixesFromFile(string filepath)
        {
            TrackChanges = TrackChangesOption.No;

            string[] allLines = File.ReadAllLines(filepath);
            string[] line = null;

            foreach (var i in allLines)
            {
                try
                {
                    line = i.Split(',');

                    if (line.Length >= 3)
                    {
                        RouteFindingCore.WptList.AddWpt(new WptNeighbor(new Waypoint(line[0], Convert.ToDouble(line[1]), Convert.ToDouble(line[2]))));
                    }

                }
                catch (Exception ex)
                {
                    WriteToLog(ex);
                    //TODO: Write to log file. Show to user, etc.
                    throw;
                }
            }
            TrackChanges = TrackChangesOption.Yes;
        }
        
        public void AddWpt(WptNeighbor item)
        {
            searchHelper.Add(item.Waypoint.ID, content.Count);
            content.Add(item);
            numNodeFrom.Add(0);
        }
        
        public void AddNeighbor(int index, Neighbor item)
        {
            if (currentTracker != null)
            {
                currentTracker.AddNeighborRecord(index);
            }
            content[index].GetNeighborList(token).Add(item);
            numNodeFrom[item.Index]++;
        }
        
        public void Clear()
        {
            content.Clear();
            searchHelper.Clear();
            numNodeFrom.Clear();
            trackerCollection.Clear();
            currentTracker = null;
        }

        public static bool TokenMatches(object item)
        {
            return token.Equals(item);
        }

        public WptNeighbor this[int index]
        {
            get
            {
                return content[index];
            }
        }

        public WptNeighbor ElementAt(int index)
        {
            return content[index];
        }

        public Waypoint WaypointAt(int index)
        {
            return content[index].Waypoint;
        }

        public LatLon LatLonAt(int index)
        {
            return content[index].Waypoint.LatLon;
        }

        public int NumberOfNodeFrom(int index)
        {
            return numNodeFrom[index];
        }

        public int Count
        {
            get { return content.Count; }
        }

        /// <summary>
        /// Find the index of WptNeighbor by ident of a waypoint.
        /// </summary>
        public int FindByID(string ident)
        {
            return searchHelper[ident];
        }

        /// <summary>
        /// Find all WptNeighbors by ident of a waypoint.
        /// </summary> 
        public List<int> FindAllByID(string ident)
        {
            return searchHelper.AllMatches(ident);
        }

        /// <summary>
        /// Find the index of WptNeighbor matching the waypoint.
        /// </summary>
        public int FindByWaypoint(string ident, double lat, double lon)
        {
            return FindByWaypoint(new Waypoint(ident, lat, lon));
        }

        /// <summary>
        /// Find the index of WptNeighbor matching the waypoint.
        /// </summary>
        public int FindByWaypoint(Waypoint wpt)
        {
            var candidates = searchHelper.AllMatches(wpt.ID);

            if (candidates != null)
            {
                foreach (int i in candidates)
                {
                    if (content[i].Waypoint.Equals(wpt))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        /// <summary>
        /// Find all occurences of WptNeighbor matching the waypoint.
        /// </summary>
        public List<int> FindAllByWaypoint(Waypoint wpt)
        {
            var candidates = searchHelper.AllMatches(wpt.ID);
            List<int> results = new List<int>();

            foreach (int i in candidates)
            {
                if (content[i].Waypoint.Equals(wpt))
                {
                    results.Add(i);
                }
            }
            return results;
        }

        public WptNeighbor Last()
        {
            return new WptNeighbor(content.Last());
        }
        
        private void revertChanges(ChangeCategory para)
        {
            //If _trackChanges is not set to "no" yet, we end current session.
            TrackChanges = TrackChangesOption.No;

            if (trackerCollection.Count > 0)
            {
                for (int i = trackerCollection.Count - 1; i >= 0; i--)
                {
                    if (trackerCollection[i].Category == para && trackerCollection[i].RegionEnd >= 0)
                    {
                        //remove neighbors first
                        var addedNeighbor = trackerCollection[i].AddedNeighbor;

                        for (int j = addedNeighbor.Count - 1; j >= 0; j--)
                        {
                            var neighbors = content[addedNeighbor[j]].GetNeighborList(token);
                            numNodeFrom[neighbors[neighbors.Count - 1].Index] --;
                            neighbors.RemoveAt(neighbors.Count - 1);
                        }

                        // Remove all wpts between regionStart and regionEnd.
                        int regionStart = trackerCollection[i].RegionStart;
                        int regionEnd = trackerCollection[i].RegionEnd;

                        if (regionEnd <= content.Count - 1)
                        {
                            for (int k = regionStart; k <= regionEnd; k++)
                            {
                                foreach (var m in content[k].GetNeighborList(token))
                                {
                                    numNodeFrom[m.Index] --;
                                }
                                searchHelper.Remove(content[k].Waypoint.ID, k, HashMap<string, int>.RemoveParameter.RemoveFirst);
                                RouteFindingCore.WptFinder.Remove(k);
                            }
                            content.RemoveRange(regionStart, regionEnd - regionStart + 1);
                            numNodeFrom.RemoveRange(regionStart, regionEnd - regionStart + 1);
                        }

                        trackerCollection.RemoveAt(i);
                    }
                }
            }

            TrackChanges = TrackChangesOption.Yes;
        }

        public void Restore()
        {
            revertChanges(ChangeCategory.Normal);
        }

        public void DisableNATs()
        {
            revertChanges(ChangeCategory.Nats);
        }

        public void DisablePacots()
        {
            revertChanges(ChangeCategory.Pacots);
        }

        public void DisableAusots()
        {
            revertChanges(ChangeCategory.Ausots);
        }

        public enum TrackChangesOption
        {
            Yes,
            No,
            AddingNATs,
            AddingPacots,
            AddingAusots
        }

        public LatLonSearchUtility<int> GenerateSearchGrids()
        {
            LatLonSearchUtility<int> searchGrid = new LatLonSearchUtility<int>(1, 5, RouteFindingCore.WptList.LatLonAt);

            for (int i = 0; i < content.Count; i++)
            {
                searchGrid.Add(i);
            }

            return searchGrid;
        }

        public double Distance(int index1, int index2)
        {
            return GreatCircleDistance(content[index1].Waypoint.Lat, content[index1].Waypoint.Lon,
                                       content[index2].Waypoint.Lat, content[index2].Waypoint.Lon);
        }

    }
}