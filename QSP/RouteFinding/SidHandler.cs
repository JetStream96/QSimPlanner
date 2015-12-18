using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static QSP.Core.QspCore;
using static QSP.LibraryExtension.Lists;
using static QSP.RouteFinding.RouteFindingCore;
using static QSP.Utilities.ErrorLogger;

namespace QSP.RouteFinding
{
    public class SidHandler
    {
        private string[] terminalProcedures;  // The lines of the entire file of, e.g KLAX.txt in \PROC folder.
        private string icao;
        private WaypointList wptList;
        private AirportManager airportList;

        public SidHandler(string icao) : this(icao, AppSettings.NavDBLocation, WptList, AirportList)
        {
        }

        /// <param name="navDBLocation">The file path, which is e.g., PROC\RCTP.txt\</param>
        /// <exception cref="LoadSidFileException"></exception>
        public SidHandler(string icao, string navDBLocation, WaypointList wptList, AirportManager airportList)
        {
            string fileLocation = navDBLocation + "\\PROC\\" + icao + ".txt";

            try
            {
                this.terminalProcedures = File.ReadAllLines(fileLocation);
            }
            catch (Exception ex)
            {
                throw new LoadSidFileException("Failed to read " + fileLocation + ".", ex);
            }
            this.icao = icao;
            this.wptList = wptList;
            this.airportList = airportList;
        }

        /// <param name="navDBLocation">The file path, which is e.g., PROC\RCTP.txt\</param>
        public SidHandler(string icao, string[] terminalProcedures, WaypointList wptList, AirportManager airportList)
        {
            this.terminalProcedures = terminalProcedures;
            this.icao = icao;
            this.wptList = wptList;
            this.airportList = airportList;
        }

        /// <summary>
        /// Find all SID available for the runway. Each transition is counted as different. 
        /// If non is available a list with 0 element will be returned.
        /// </summary>
        /// <param name="rwy">Runway Ident</param>
        public List<string> GetSidList(string rwy)
        {
            //after user selected an rwy, get a list of all sids available
            //first get all lines starting with "SID"

            var result = new List<string>();
            string[] allLines = terminalProcedures; 
            string[] line = null;
            var sidsWithTransition = new List<string>();

            foreach (var i in allLines)
            {
                if (i == null)
                {
                    continue;
                }

                line = i.Split(',');

                if (line.Length >= 3 && line[0] == "SID")
                {
                    if (line[2] == rwy || (line[2] == "ALL" && Utilities.HasRwySpecificPart(allLines, line[1]) == false))
                    {
                        result.Add(line[1]);
                    }
                    else if (Utilities.IsRwyIdent(line[2]) == false && result.IndexOf(line[1]) != -1 && line[2] != "ALL")
                    {
                        //since the records of transitions come AFTER the rwy specific and common part,
                        //only transitions with their main SID already in result list are considered

                        sidsWithTransition.Add(line[1]);
                        result.Add(line[1] + "." + line[2]);
                    }
                }
            }

            result = result.WithoutDuplicates();
            sidsWithTransition = sidsWithTransition.WithoutDuplicates();

            foreach (var i in sidsWithTransition)
            {
                result.Remove(i);
            }

            return result;
        }

        /// <summary>
        /// Add necessary waypoints and neighbors for SID computation to WptList, and returns the index of Orig. rwy in WptList.
        /// </summary>
        public int AddSidsToWptList(string rwy, List<string> sid)
        {
            // This breaks down into 4 cases:
            // 1. There's no SID at all. 
            // 2. The last "waypoint" (i.e. instruction) of the SID is a vector.
            // 3. The last waypoint is not vector but it's not connected to an airway (i.e. not in WptList).
            // 4. The last waypoint is connected to an airway. 

            if (sid.Count == 0)
            {
                //case 1
                //if the sid list is empty, find nearby wpts and use DCT

                var rwyLatLon = airportList.RwyLatLon(icao, rwy);
                var nearbyWpts = Utilities.sidStarToAirwayConnection("DCT", rwyLatLon, 0);

                var wpt = new Waypoint(icao + rwy, rwyLatLon);
                wptList.AddWpt(new WptNeighbor(wpt, nearbyWpts));
            }
            else
            {
                //case 2, 3, 4
                var neighbors = new List<Neighbor>();
                //now we get a list of neighbors of dep. runway

                foreach (var i in sid)
                {
                    try
                    {
                        neighbors.AddRange(getSidEndPoints(rwy, i));
                        // this is where case 2, 3, 4 are handled.
                    }
                    catch (WaypointNotFoundException ex)
                    {
                        WriteToLog(ex.ToString());
                    }
                }

                var wptToAdd = new WptNeighbor(new Waypoint(icao + rwy, airportList.RwyLatLon(icao, rwy)), 
                                               neighbors);
                wptList.AddWpt(wptToAdd);

            }
            return wptList.Count - 1;
        }

        /// <summary>
        /// Gets a tuple containing the name of SID/STAR and transition.
        /// </summary>
        public static Tuple<string, string> SplitSidStarTransition(string sidStar)
        {
            string sidName = null;
            string transName = null;

            if (sidStar.IndexOf('.') != -1)
            {
                sidName = sidStar.Substring(0, sidStar.IndexOf('.'));
                transName = sidStar.Substring(sidStar.IndexOf('.') + 1);
            }
            else
            {
                sidName = sidStar;
                transName = "";
            }
            return new Tuple<string, string>(sidName, transName);
        }

        /// <summary>
        /// Returns a list of waypoints, starting from the dep. runway to the last waypoint of transition (if there is one). 
        /// The boolean indicates whether the last waypoint is a vector.
        /// </summary>
        private Tuple<List<Waypoint>, bool> importSidFromFile(string rwy, string sid)
        {
            string[] allLines = terminalProcedures;

            var sidNameSplit = SplitSidStarTransition(sid);
            var sidName = sidNameSplit.Item1;
            var transName = sidNameSplit.Item2;

            var sidWptComplete = getAllWptSidComplete(allLines, sidName, transName, rwy);
            var sidWpts = new List<Waypoint>();
            sidWpts.Add(new Waypoint(icao + rwy, airportList.RwyLatLon(icao, rwy)));
            sidWpts.AddRange(sidWptComplete.Item1);

            return new Tuple<List<Waypoint>, bool>(sidWpts, sidWptComplete.Item2);
        }
               
        /// <exception cref="WaypointNotFoundException"></exception>
        private List<Neighbor> getSidEndPoints(string rwy, string sid)
        {
            var importResult = importSidFromFile(rwy, sid);
            var sidWpts = importResult.Item1;
            bool lastWptIsVector = importResult.Item2;

            var endPoints = new List<Neighbor>();

            if (lastWptIsVector)
            {
                //case 2: the last wpt is a vector
                //Then find some nearby navaids to join an airway

                endPoints = Utilities.sidStarToAirwayConnection(sid, sidWpts.Last().LatLon, Utilities.GetTotalDistance(sidWpts));
                // he sid will be displayed like: EWR1 JERSY [airway] ... to the user
            }
            else
            {
                //case 3, 4
                var lastWptSid = new Neighbor();
                lastWptSid.Index = wptList.FindByWaypoint(sidWpts.Last());

                if (lastWptSid.Index < 0)
                {
                    throw new WaypointNotFoundException("Waypoint " + sidWpts.Last() + " is not found.");
                }

                if (wptList[lastWptSid.Index].Neighbors.Count == 0)
                {
                    //case 3: the endpoint is a waypoint, not a vector, but this wpt cannnot be found in ats.txt
                    //in this case we try to find a nearby wpt to direct to 
                    //the sid should be displayed, as [sid name] [endpoint] DCT [the nearby wpt we find] [airway] ...

                    //the last waypoint is added to WptList, where its neighbors are nearby waypoints connected to an airway
                    foreach (var k in Utilities.sidStarToAirwayConnection("DCT", sidWpts.Last().LatLon, 0))
                    {
                        wptList.AddNeighbor(lastWptSid.Index, k);
                    }
                }

                //for both case 3 and 4
                lastWptSid.Distance = Utilities.GetTotalDistance(sidWpts);
                lastWptSid.Airway = sid;

                endPoints.Add(lastWptSid);
            }
            return endPoints;
        }

        /// <summary>
        /// Gets all waypoints in a specific SID, given an array of lines of text which contains the SID procedure.
        /// The text should be a part of, e.g. PROC\RCTP.txt
        /// The bool indicates whether the last waypoint is vector.
        /// </summary>
        /// <param name="allLines"></param>
        /// <param name="startLineNum">The particular SID description should starts at this line number.</param>
        private Tuple<List<Waypoint>, bool> findAllWptInSidLineNum(string[] allLines, int startLineNum)
        {
            //To get all wpts from this section:
            //
            //SID, SU1T, 23R, 1
            //VD, ,0,0,0,TIA,0.0,0.0,233.0,3.0,0,0,0,0,0,0,0,0, 
            //VI,0, ,0.0,275.0,0,0,0,0,0,0,0,0, 
            //FD,TIA,25.088278,121.233000,0,TIA,0.0,13.0,245.0,13.0,2,4000,0,0,0,0,0,0, 
            //CF,CHALI,24.729431,120.508295,0,TIA,245.7,45.0,245.0,32.0,0,0,0,0,0,0,0,0, 

            bool lastWptIsVector = false;
            int j = 0;
            var result = new List<Waypoint>();

            while (allLines[j + startLineNum].IndexOf(",") != -1)
            {
                //record all the lines in this sid
                string[] line = allLines[j + startLineNum].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (Utilities.HasCorrds(line[0]) == 0)
                {
                    lastWptIsVector = false;
                    result.Add(new Waypoint(line[1], Convert.ToDouble(line[2]), Convert.ToDouble(line[3])));
                }
                else
                {
                    lastWptIsVector = true;
                }
                j++;
            }
            return new Tuple<List<Waypoint>, bool>(result, lastWptIsVector);
        }

        private Tuple<List<Waypoint>, bool> getAllWptSid(string[] allLines, string sid, string rwyIdent)
        {
            for (int i = 0; i < allLines.Length; i++)
            {
                //search each line for the desired sid
                string[] line = allLines[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (line.Length >= 3 && line[0] == "SID" && line[1] == sid && line[2] == rwyIdent)
                {
                    //if sid is found
                    return findAllWptInSidLineNum(allLines, i + 1);
                }

            }
            return null;
        }

        private Tuple<List<Waypoint>, bool> getAllWptSidComplete(string[] allLines, string sidName, string transName, string rwyIdent)
        {
            var wpts = new List<Waypoint>();
            bool lastWptIsVector = false;

            var sidWptsRwySpecific = getAllWptSid(allLines, sidName, rwyIdent);

            if (sidWptsRwySpecific != null)
            {
                wpts.AddRange(sidWptsRwySpecific.Item1);
                lastWptIsVector = sidWptsRwySpecific.Item2;
            }

            var sidWptsCommon = getAllWptSid(allLines, sidName, "ALL");
            //the common part may not exist

            if (sidWptsCommon != null)
            {
                wpts.AddRange(sidWptsCommon.Item1);
                lastWptIsVector = sidWptsCommon.Item2;
            }

            if (transName != "")
            {
                var sidTrans = getAllWptSid(allLines, sidName, transName);
                wpts.AddRange(sidTrans.Item1);
                lastWptIsVector = sidTrans.Item2;
            }

            return new Tuple<List<Waypoint>, bool>(wpts.WithoutDuplicates(), lastWptIsVector);
        }

        /// <summary>
        /// Returns total distance of the SID and the last wpt, regardless whether the last wpt is in wptList.
        /// If there isn't any waypoint in the SID (e.g. a vector after takeoff), this returns 0 for distance 
        /// and runway for waypoint (e.g. KLAX25L).
        /// </summary>
        public Tuple<double, Waypoint> InfoForAnalysis(string rwy, string sid)
        {
            var importResult = importSidFromFile(rwy, sid);
            return new Tuple<double, Waypoint>(Utilities.GetTotalDistance(importResult.Item1), importResult.Item1.Last());
        }
    }
}
