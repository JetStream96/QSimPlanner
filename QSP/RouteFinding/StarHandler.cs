using QSP.AviationTools;
using QSP.RouteFinding.Containers;
using System;
using System.Collections.Generic;
using System.IO;
using static QSP.Core.QspCore;
using static QSP.LibraryExtension.Lists;
using static QSP.RouteFinding.RouteFindingCore;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding
{

    public class StarHandler
    {
        private string filePath;
        private string icao;
        private TrackedWptList wptList;
        private AirportManager airportList;

        public StarHandler(string icao) : this(icao, AppSettings.NavDBLocation, WptList, AirportList)
        {
        }

        /// <param name="navDBLocation">The file path, which is e.g., PROC\RCTP.txt\</param>
        public StarHandler(string icao, string navDBLocation, TrackedWptList wptList, AirportManager airportList)
        {
            filePath = navDBLocation + "\\PROC\\" + icao + ".txt";
            this.icao = icao;
            this.wptList = wptList;
            this.airportList = airportList;
        }

        private List<string> rwyCompatibleStarsNoTrans(string[] allLines, string rwy)
        {
            var result = new List<string>();
            string[] line = null;

            foreach (var i in allLines)
            {
                if (i == null)
                {
                    continue;
                }
                line = i.Split(',');

                if (line.Length >= 3 && line[0] == "STAR")
                {
                    if (line[2] == rwy || (line[2] == "ALL" && Utilities.HasRwySpecificPart(allLines, line[1]) == false))
                    {
                        result.Add(line[1]);
                    }
                }
            }
            return result;
        }

        public List<string> GetStarList(string rwy)
        {
            //after user selected an rwy, get a list of all stars available
            //first get all lines starting with "STAR"
            //"ALL" means a common part for all runways that can use that STAR (does not mean the STAR is avail for all rwys)

            var result = new List<string>();
            string[] allLines = File.ReadAllLines(filePath);
            string[] line = null;

            var starWithTransition = new List<string>();
            result = rwyCompatibleStarsNoTrans(allLines, rwy);

            foreach (var i in allLines)
            {
                if (i == null)
                {
                    continue;
                }
                line = i.Split(',');

                if (line.Length >= 3 && line[0] == "STAR")
                {
                    if (Utilities.IsRwyIdent(line[2]) == false && result.IndexOf(line[1]) != -1 && line[2] != "ALL")
                    {
                        starWithTransition.Add(line[1]);
                        result.Add(line[1] + "." + line[2]);
                    }
                }
            }

            result = result.WithoutDuplicates();
            starWithTransition = starWithTransition.WithoutDuplicates();

            foreach (var i in starWithTransition)
            {
                result.Remove(i);
            }
            return result;
        }

        /// <summary>
        /// Add necessary waypoints for STAR computation to WptList, and returns the index of Dest. rwy in WptList.
        /// </summary>
        public int AddStarsToWptList(string rwy, List<string> star)
        {
            //There are 3 cases:
            //1. No STAR at all. (i.e. the star list is empty)
            //2. The first wpt in star is NOT connected to an airway (and is therefore not in wptList).
            //3. The first wpt in star IS connected to an airway.

            int DEST_RWY_INDEX = wptList.Count;

            var rwyLatLon = airportList.RwyLatLon(icao, rwy);
            wptList.AddWpt(icao + rwy, rwyLatLon.Lat, rwyLatLon.Lon);

            if (star.Count == 0)
            {
                //case 1: the star list is empty, find nearby wpts and use DCT

                var nearbyWpts = Utilities.sidStarToAirwayConnection("", rwyLatLon, 0);

                foreach (var i in nearbyWpts)
                {
                    wptList.AddNeighbor(i.Index, new Neighbor(wptList.Count - 1, "DCT", i.Distance));
                }
            }
            else
            {
                //case 2, 3
                foreach (var i in star)
                {
                    addStarWpts(i, rwy, DEST_RWY_INDEX);
                }
            }
            return DEST_RWY_INDEX;
        }


        private void addStarWpts(string star, string rwy, int DEST_RWY_INDEX)
        {
            var analysisInfo = InfoForAnalysis(rwy, star);
            Waypoint firstWpt = analysisInfo.Item2;
            double starDis = analysisInfo.Item1;

            int firstWptIndex = wptList.FindByWaypoint(firstWpt);
            //get index of starting wpt

            var destRwyAsNeighbor = new Neighbor(DEST_RWY_INDEX, star, starDis);

            if (wptList.NumberOfNodeFrom(firstWptIndex) == 0)
            {
                //case 2: when the connecting wpt(i.e. the first wpt in the star) is not found in ats.txt

                //add the first wpt to wptList
                wptList.AddNeighbor(firstWptIndex, destRwyAsNeighbor);

                //now find nearby waypoints of firstWpt of star
                var nearbyWpts = Utilities.sidStarToAirwayConnection("DCT", firstWpt.LatLon, 0);

                foreach (var pt in nearbyWpts)
                {
                    //each pt directs to the firstWpt of star
                    wptList.AddNeighbor(pt.Index, new Neighbor(firstWptIndex, "DCT", pt.Distance));
                }
            }
            else
            {
                //case 3
                //add dest rwy as a neighbor of firstWpt of STAR
                wptList.AddNeighbor(firstWptIndex, destRwyAsNeighbor);
            }
        }

        /// <summary>
        /// Gets all waypoints in a specific STAR, given an array of lines of text which contains the STAR procedure.
        /// The text should be a part of, e.g. PROC\RCTP.txt
        /// The second item in the tuple is the first waypoint of the STAR.
        /// </summary>
        private Tuple<List<LatLon>, Waypoint> getStarLatLonLineNum(string[] allLines, int startLineNum)
        {
            Waypoint firstWpt = null;
            var latLonList = new List<LatLon>();
            int i = 0;
            string[] line = null;

            while (allLines[startLineNum + i].IndexOf(',') >= 0)
            {
                line = allLines[startLineNum + i].Split(',');

                if (Utilities.HasCorrds(line[0]) == 0)
                {
                    if (latLonList.Count == 0)
                    {
                        firstWpt = new Waypoint(line[1], Convert.ToDouble(line[2]), Convert.ToDouble(line[3]));
                    }

                    latLonList.Add(new LatLon(Convert.ToDouble(line[2]), Convert.ToDouble(line[3])));
                }
                i++;
            }
            return new Tuple<List<LatLon>, Waypoint>(latLonList, firstWpt);
        }

        /// <summary>
        /// Gets all waypoints in a specific STAR, given an array of lines of text which contains the STAR procedure.
        /// The text should be a part of, e.g. PROC\RCTP.txt
        /// The second item in the tuple is the first waypoint of the STAR.
        /// </summary>
        private Tuple<List<LatLon>, Waypoint> getStarLatLon(string[] allLines, string starNoTransPart, string rwy)
        {
            //rwy can be "ALL" or transition
            string[] line = null;

            for (int i = 0; i <= allLines.Length - 1; i++)
            {
                line = allLines[i].Split(',');

                if (line.Length >= 3 && line[0] == "STAR" && line[1] == starNoTransPart && line[2] == rwy)
                {
                    return getStarLatLonLineNum(allLines, i + 1);
                }
            }
            return null;
        }

        public Tuple<double, Waypoint> InfoForAnalysis(string rwy, string star)
        {
            string[] allLines = File.ReadAllLines(filePath);
            var latLonList = new List<LatLon>();
            Waypoint firstWpt = null;

            var splitResult = SidHandler.SplitSidStarTransition(star);

            string starNoTransPart = splitResult.Item1;
            string starTrans = splitResult.Item2;

            //some of these might be Nothing. e.g. if the there is no transition, transPart would be Nothing.
            var transPart = getStarLatLon(allLines, starNoTransPart, starTrans);
            var commonPart = getStarLatLon(allLines, starNoTransPart, "ALL");
            var rwySpecificPart = getStarLatLon(allLines, starNoTransPart, rwy);

            var nonEmpty = new List<Tuple<List<LatLon>, Waypoint>>();

            if (transPart != null)
            {
                nonEmpty.Add(transPart);
            }
            else if (commonPart != null)
            {
                nonEmpty.Add(commonPart);
            }
            else if (rwySpecificPart != null)
            {
                nonEmpty.Add(rwySpecificPart);
            }
            else
            {
                //TODO: exception
            }

            firstWpt = nonEmpty[0].Item2;

            foreach (var i in nonEmpty)
            {
                latLonList.AddRange(i.Item1);
            }

            latLonList.Add(airportList.RwyLatLon(icao, rwy));

            return new Tuple<double, Waypoint>(Utilities.GetTotalDistance(latLonList), firstWpt);

        }

    }

}
