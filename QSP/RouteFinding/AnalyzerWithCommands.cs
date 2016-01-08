using QSP.AviationTools;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static QSP.RouteFinding.Constants;
using static QSP.RouteFinding.RouteFindingCore;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding
{

    public class AnalyzerWithCommands
    {
        private const string autoCommand = "AUTO";
        private const string randCommand = "RAND";

        private static Waypoint emptyWpt = new Waypoint("");
        private string origIcao;
        private string origRwy;
        private string destIcao;
        private string destRwy;

        private string routeInput;
        private LatLon origLatLon;
        private LatLon destLatLon;

        public AnalyzerWithCommands(string origIcao, string origRwy, string destIcao, string destRwy, string routeInput)
        {
            this.origIcao = origIcao;
            this.origRwy = origRwy;
            this.destIcao = destIcao;
            this.destRwy = destRwy;
            this.routeInput = routeInput;

            setLatLons();
        }


        private void setLatLons()
        {
            origLatLon = AirportList.AirportLatlon(origIcao);
            destLatLon = AirportList.AirportLatlon(destIcao);
        }

        public Route Parse()
        {

            string[] input = routeInput.Split(RouteAnalyzer.Delimiters, StringSplitOptions.RemoveEmptyEntries);

            foreach (int j in commandLocations(input, randCommand))
            {
                parseRandCommand(input, j);
            }

            foreach (int i in commandLocations(input, autoCommand))
            {
                parseAutoCommand(input, i);
            }

            string completeRouteStr = string.Join(" ", input);
            var analyzer = new RouteAnalyzer(origIcao, origRwy, destIcao, destRwy, completeRouteStr);

            return analyzer.Parse();

        }

        #region "Parse Rand"

        /// <param name="index">Index of "RAND" in input() array.</param>
        private void parseRandCommand(string[] input, int index)
        {
            string asMiddle = tryParseRandAsMiddle(input, index);

            if (asMiddle != null)
            {
                input[index] = asMiddle;
                return;
            }
            string asFirst = tryParseRandAsFirst(input, index);

            if (asFirst != null)
            {
                input[index] = asFirst;
                return;
            }
            string asLast = tryParseRandAsLast(input, index);

            if (asLast != null)
            {
                input[index] = asLast;
                return;
            }
            throw new ArgumentException();
        }

        private string randRouteStr(LatLon pt1, LatLon pt2)
        {
            if (pt1.Distance(pt2) <= 500)
            {
                return "DCT";
            }
            var randFinder = new RandomRouteFinder(pt1, pt2);
            return randRouteStr(randFinder.Find());
        }

        /// <param name="index">Index of "RAND" in input() array.</param>
        private string tryParseRandAsMiddle(string[] input, int index)
        {
            if (index >= 1 && index + 1 < input.Length)
            {
                var pt1 = WptList[selectWptSameIdent(input[index - 1])].LatLon;
                var pt2 = WptList[selectWptSameIdent(input[index + 1])].LatLon;

                return randRouteStr(pt1, pt2);
            }
            return null;
        }

        /// <param name="index">Index of "RAND" in input() array.</param>
        private string tryParseRandAsFirst(string[] input, int index)
        {
            //auto find SID (approximately optimum solution)
            //it's possible that first element in input() is orig airport

            if ((index == 0 && input.Length >= 2) || (input.Length >= 3 && index == 1 && input[0] == origIcao))
            {
                string nextWpt = input[index + 1];

                //find all sids
                var sidManager = new SidHandler(origIcao);
                var sidList = sidManager.GetSidList(origRwy);
                string origRwyWpt = origIcao + origRwy;
                var endPoints = new List<SidInfo>();
                LatLon nextLatLon = null;
                int p = 0;

                //if sid is avail.

                if (sidList.Count > 0)
                {
                    SidInfo analysisInfo = null;

                    foreach (var k in sidList)
                    {
                        analysisInfo = sidManager.InfoForAnalysis(origRwy, k);

                        if (analysisInfo.LastWaypoint.ID != origRwyWpt)
                        {
                            endPoints.Add(analysisInfo);
                        }
                    }

                    if (endPoints.Count > 0)
                    {
                        nextLatLon = WptList[selectWptSameIdent(nextWpt)].LatLon;
                        p = selectSid(endPoints.ToArray(), nextLatLon);

                        return sidList[p] + " " + endPoints[p].LastWaypoint.ID + " " + randRouteStr(endPoints[p].LastWaypoint.LatLon, nextLatLon);
                    }
                }

                //no sid for the rwy/airport, e.g. KORD
                //or sid contains nothing but a vector
                var nearbyPts = WaypointAirwayConnector.FindAirwayConnection(origLatLon, WptList);

                // TODO: This is not quite correct.
                for (int k = 0; k <= sidList.Count - 1; k++)
                {
                    endPoints.Add(new SidInfo(nearbyPts[k].Distance, WptList[nearbyPts[k].Index], false));
                }

                nextLatLon = WptList[selectWptSameIdent(nextWpt)].LatLon;
                p = selectSid(endPoints.ToArray(), nextLatLon);

                return sidList[p] + " " + endPoints[p].LastWaypoint.ID + " " + randRouteStr(endPoints[p].LastWaypoint.LatLon, nextLatLon);
            }

            //this is not to be parsed as first 
            return null;

        }

        /// <param name="index">Index of "RAND" in input() array.</param>
        private string tryParseRandAsLast(string[] input, int index)
        {
            int lastIndex = input.Length - 1;

            //auto find STAR 
            //it's possible that last element in input() is dest airport

            if ((index == lastIndex && input.Length >= 2) ||
                (input.Length >= 3 && index == lastIndex - 1 && input[lastIndex] == destIcao))
            {
                string prevWpt = input[index - 1];

                //find all stars
                var starManager = new StarHandler(destIcao);
                var starList = starManager.GetStarList(destRwy);

                StarInfo[] endPoints = null;

                if (starList.Count > 0)
                {
                    //if star is avail.

                    endPoints = new StarInfo[starList.Count];

                    for (int k = 0; k <= starList.Count - 1; k++)
                    {
                        endPoints[k] = starManager.InfoForAnalysis(destRwy, starList[k]);
                    }
                }
                else
                {
                    //no star for the rwy/airport

                    var nearbyPts = WaypointAirwayConnector.FindAirwayConnection(destLatLon, WptList);
                    endPoints = new StarInfo[nearbyPts.Count];

                    // TODO: This is not quite correct.
                    for (int k = 0; k < starList.Count; k++)
                    {
                        endPoints[k] = new StarInfo(nearbyPts[k].Distance, WptList[nearbyPts[k].Index]);
                    }
                }

                var prevLatLon = WptList[selectWptSameIdent(prevWpt)].LatLon;
                int p = selectStar(endPoints, prevLatLon);
                var firstPt = endPoints[p].FirstWaypoint;

                return randRouteStr(prevLatLon, firstPt.LatLon) + " " + firstPt.ID + " " + starList[p];
            }

            return null;
            //this is not to be parsed as last

        }

        private string randRouteStr(List<LatLon> item)
        {
            var s = new StringBuilder();

            for (int i = 1; i <= item.Count - 2; i++)
            {
                s.Append(ToWptIdent(item[i]));
                s.Append(" ");
            }

            s.Remove(s.Length - 1, 1);

            return s.ToString();

        }

        #endregion

        private static int selectStar(StarInfo[] procCollection, LatLon prevPtLatLon)
        {
            int result = 0;
            double minDis = MAX_DIS;
            double dis = 0.0;

            for (int i = 0; i <= procCollection.Count() - 1; i++)
            {
                var proc = procCollection[i];
                dis = proc.TotalDistance + prevPtLatLon.Distance(proc.FirstWaypoint.Lat, proc.FirstWaypoint.Lon);
                if (dis < minDis)
                {
                    result = i;
                    minDis = dis;
                }
            }
            return result;
        }

        private static int selectSid(SidInfo[] procCollection, LatLon prevPtLatLon)
        {
            int result = 0;
            double minDis = MAX_DIS;
            double dis = 0.0;

            for (int i = 0; i <= procCollection.Count() - 1; i++)
            {
                var proc = procCollection[i];
                dis = proc.TotalDistance + prevPtLatLon.Distance(proc.LastWaypoint.Lat, proc.LastWaypoint.Lon);
                if (dis < minDis)
                {
                    result = i;
                    minDis = dis;
                }
            }
            return result;
        }

        private static char StandardIdent(double lat, double lon)
        {
            if (lat >= 0)
            {
                if (lon >= 0)
                {
                    return 'E';
                }
                return 'N';
            }
            else
            {
                if (lon >= 0)
                {
                    return 'S';
                }
                return 'W';
            }
        }

        // TODO: potential rounding error issue.
        private static string ToWptIdent(LatLon latLon)
        {
            if (latLon.Lat % 1 == 0.0 && latLon.Lon % 1 == 0.0)
            {
                if (Math.Abs(latLon.Lon) >= 100)
                {
                    return Math.Abs(latLon.Lat).ToString().PadLeft(2, '0') +
                           StandardIdent(latLon.Lat, latLon.Lon) +
                           (Math.Abs(latLon.Lon) - 100).ToString().PadLeft(2, '0');
                }
                else
                {
                    return Math.Abs(latLon.Lat).ToString().PadLeft(2, '0') +
                           Math.Abs(latLon.Lon).ToString().PadLeft(2, '0') +
                           StandardIdent(latLon.Lat, latLon.Lon);
                }
            }
            else
            {
                //TODO: ??
                throw new ArgumentException();
            }
        }

        #region "Parse Auto"

        /// <param name="index">Index of "AUTO" in input() array.</param>
        private void parseAutoCommand(string[] input, int index)
        {
            //trying to parse as a waypoint in the middle
            string asMiddle = tryParseAutoAsMiddle(input, index);

            if (asMiddle != null)
            {
                input[index] = asMiddle;
                return;
            }

            //trying to parse as if AUTO is the first in the route
            string asFirst = tryParseAutoAsFirst(input, index);

            if (asFirst != null)
            {
                input[index] = asFirst;
                return;
            }

            //trying to parse as if AUTO is the last in the route
            string asLast = tryParseAutoAsLast(input, index);

            if (asLast != null)
            {
                input[index] = asLast;
                return;
            }

            throw new ArgumentException();
        }


        /// <param name="index">Index of "AUTO" in input() array.</param>
        private string tryParseAutoAsMiddle(string[] input, int index)
        {
            if (index >= 1 && index + 1 < input.Length)
            {
                var rte = new RouteFinder().FindRoute(selectWptSameIdent(input[index - 1]), selectWptSameIdent(input[index + 1]));
                rte.Waypoints[0] = emptyWpt;
                rte.Waypoints[rte.Waypoints.Count - 1] = emptyWpt;

                return rte.ToString(false, false,Route.TracksDisplayOption.Collapse);
            }
            return null;
        }

        /// <param name="index">Index of "AUTO" in input() array.</param>
        private string tryParseAutoAsFirst(string[] input, int index)
        {
            string nextWpt = null;

            //auto find SID 
            //it's possible that first element in input() is orig airport

            if ((index == 0 && input.Length >= 2) || (input.Length >= 3 && index == 1 && input[0] == origIcao))
            {
                nextWpt = input[index + 1];

                //find all sids
                var sidManager = new SidHandler(origIcao);
                var sidList = sidManager.GetSidList(origRwy);

                var rte = new RouteFinder().FindRoute(origIcao, origRwy, sidList, selectWptSameIdent(input[index + 1]));
                rte.Waypoints[rte.Waypoints.Count - 1] = emptyWpt;

                return rte.ToString(false, false, Route.TracksDisplayOption.Collapse);

            }

            //this is not to be parsed as first 
            return null;

        }

        /// <param name="index">Index of "AUTO" in input() array.</param>
        private string tryParseAutoAsLast(string[] input, int index)
        {
            int lastIndex = input.Length - 1;
            string prevWpt = null;

            //auto find STAR 
            //it's possible that last element in input() is dest airport

            if ((index == lastIndex && input.Length >= 2) || (input.Length >= 3 && index == lastIndex - 1 && input[lastIndex] == destIcao))
            {
                prevWpt = input[index - 1];

                //find all stars
                var starManager = new StarHandler(destIcao);
                var starList = starManager.GetStarList(destRwy);

                var rte = new RouteFinder().FindRoute(selectWptSameIdent(input[index - 1]), destIcao, destRwy, starList);
                rte.First.Waypoint = emptyWpt;

                return rte.ToString(false, false, Route.TracksDisplayOption.Collapse);
            }
            return null;        //this is not to be parsed as last         
        }

        #endregion

        private List<int> commandLocations(string[] input, string command)
        {
            var result = new List<int>();

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == command)
                {
                    result.Add(i);
                }
            }
            return result;
        }

        //returns index in WptList, or -1 if not found
        private int findWptList(string ident)
        {
            var searchResultWptList = WptList.FindAllByID(ident);

            if (searchResultWptList != null && searchResultWptList.Count > 0)
            {
                if (searchResultWptList.Count == 1)
                {
                    return searchResultWptList[0];
                }

                double dis = MAX_DIS;
                double elemDis = 0;
                LatLon wptLatLon = null;
                int chosenIndex = 0;

                foreach (int i in searchResultWptList)
                {
                    wptLatLon = WptList.LatLonAt(i);
                    elemDis = origLatLon.Distance(wptLatLon) + destLatLon.Distance(wptLatLon);

                    if (elemDis < dis)
                    {
                        dis = elemDis;
                        chosenIndex = i;
                    }
                }
                return chosenIndex;
            }
            else
            {
                return -1;
            }
        }

        /// <exception cref="InvalidIdentifierException"></exception>
        private int selectWptSameIdent(string ident)
        {
            int indexWptList = findWptList(ident);

            if (indexWptList >= 0)
            {
                return indexWptList;
            }
            throw new InvalidIdentifierException();
        }

    }

}
