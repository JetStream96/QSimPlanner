using QSP.RouteFinding.Containers;
using QSP.RouteFinding.TerminalProcedures.Sid;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.LibraryExtension.Arrays;
using static QSP.RouteFinding.RouteFindingCore;
using QSP.RouteFinding.TerminalProcedures.Star;
using QSP.RouteFinding.Routes;
using QSP.Core;

namespace QSP.RouteFinding
{

    // 1. Input: The string, consisting of airport icao code (ICAO), airway (AWY), waypoint (WPT) and/or direct (DCT) sysbols.
    //           These should be seperated by at least one of the following char/strings:
    //           (1) space
    //           (2) Tab
    //           (3) LF
    //           (4) CR
    //
    // 2. Not case-sensitive to input string. They get converted to upper case before parsing.
    //
    // 3. Format: ICAO SID WPT AWY WPT ... WPT STAR ICAO
    //    (1) First ICAO must be identical to origin icao code. Last ICAO must be identical to dest icao code.
    //    (2) Both ICAO can be omitted.
    //    (3) Any AWY can be replaced by DCT. The route will be a direct between the two waypoints.
    //    (4) SID/STAR can be DCT. The route will be a direct from/to airport. It can be omitted as well.
    //    (5) Any DCT can be omitted.
    //
    // 4. All cases of SID/STAR are handled. These cases are:
    //        For SID:
    //            1. There's no SID at all. 
    //            2. The last "waypoint" (i.e. instruction) of the SID is a vector.
    //            3. The last waypoint is not vector but it's not connected to an airway (i.e. not in WptList).
    //            4. The last waypoint is connected to an airway. 
    //
    //        For STAR:
    //            1. No STAR at all. (i.e. the star list is empty)
    //            2. The first wpt in star is NOT connected to an airway (and is therefore not in wptList).
    //            3. The first wpt in star IS connected to an airway.
    //
    // 5. If the format is wrong, an InvalidIdentifierException will be thrown with an message describing the place where the problem occurs.
    //
    // 6. It's not allowed to direct from one waypoint to another which is more than 500 nm away.
    //    Otherwise an exception will be thrown indicating that the latter waypoint is not a valid waypoint.

    public class RouteAnalyzer
    {
        //TODO: Small issue: A star name could be exactly the same with a wpt name.

        private static string WPT_RANGE_ERR = " Please note that any two waypoints cannot be more than 500 nm apart.";
        public static char[] Delimiters = { ' ', '\r', '\n', '\t' };
        private string origIcao;
        private string origRwy;
        private string destIcao;
        private string destRwy;
        private string routeInput;

        private Waypoint sidLastWpt;

        public RouteAnalyzer(string origIcao, string origRwy, string destIcao, string destRwy, string routeInput)
        {
            this.origIcao = origIcao;
            this.origRwy = origRwy;
            this.destIcao = destIcao;
            this.destRwy = destRwy;
            this.routeInput = routeInput;
            sidLastWpt = null;
        }

        private enum NodeType
        {
            Orig,
            Sid,
            Wpt,
            Awy,
            Star,
            Dest
        }

        private static NodeType[] nextNode(NodeType currentNode)
        {
            switch (currentNode)
            {
                case NodeType.Orig:
                    return new NodeType[] { NodeType.Wpt, NodeType.Sid };

                case NodeType.Sid:
                    return new NodeType[] { NodeType.Wpt };

                case NodeType.Wpt:
                    return new NodeType[] { NodeType.Dest, NodeType.Awy, NodeType.Wpt, NodeType.Star };

                case NodeType.Awy:
                    return new NodeType[] { NodeType.Wpt };

                case NodeType.Star:
                    return new NodeType[] { NodeType.Dest };

                case NodeType.Dest:
                    return new NodeType[] { };

                default:
                    throw new EnumNotSupportedException("Enum not supported.");
            }
        }

        public ManagedRoute Parse()
        {
            var result = new ManagedRoute(TracksInUse);
            string[] input = routeInput.ToUpper().Split(Delimiters, StringSplitOptions.RemoveEmptyEntries).RemoveElements("DCT");
            NodeType currentNode = NodeType.Orig;

            sidLastWpt = null;

            //add orig rwy
            result.AddLastWaypoint(new Waypoint(origIcao + origRwy, AirportList.RwyLatLon(origIcao, origRwy)));

            for (int i = 0; i < input.Length; i++)
            {
                NodeType[] nextNodeCandidate = nextNode(currentNode);

                if (nextNodeCandidate.Length == 0)
                {
                    break;
                }

                for (int j = 0; j < nextNodeCandidate.Length; j++)
                {
                    if (tryParse(result, currentNode, nextNodeCandidate[j], input[i]))
                    {
                        currentNode = nextNodeCandidate[j];
                        break;
                    }
                    else if (j == nextNodeCandidate.Length - 1)
                    {
                        if (currentNode == NodeType.Awy)
                        {
                            throw new InvalidIdentifierException("\"" + input[i] + "\" is not a waypoint of airway " +
                                                                 result.LastNode.Previous.Value.AirwayToNext + ".");
                        }
                        else
                        {
                            throw new InvalidIdentifierException(exceptionMsg(input[i], nextNodeCandidate) + WPT_RANGE_ERR);
                        }
                    }
                }
            }

            //add dest rwy
            result.AddLastWaypoint(new Waypoint(destIcao + destRwy, AirportList.RwyLatLon(destIcao, destRwy)));
           
            return result;
        }

        /// <summary>
        /// This function modifies the route. Returns a bool indicates whether the parse was successful.
        /// </summary>
        private bool tryParse(ManagedRoute rte, NodeType prevNode, NodeType currentNode, string text)
        {
            switch (currentNode)
            {
                case NodeType.Orig:
                    return (text == origIcao);

                case NodeType.Dest:
                    return (text == destIcao);

                case NodeType.Sid:
                    return tryParseSid(rte, text);

                case NodeType.Star:
                    return tryParseStar(rte, text);

                case NodeType.Awy:
                    return tryParseAwy(rte, prevNode, text);

                case NodeType.Wpt:
                    return tryParseWpt(rte, prevNode, text);

                default:
                    throw new ArgumentOutOfRangeException("Enum not supported.");
            }
        }

        private bool tryParseStar(ManagedRoute rte, string text)
        {
            try
            {
                var starManager = new StarHandler(destIcao);
                var starInfo = starManager.InfoForAnalysis(destRwy, text);

                // If the last waypoint is not the first waypoint of the STAR, we automatically correct this by
                // inserting the first waypoint of STAR.
                rte.Last.DistanceToNext = starInfo.TotalDistance;
                rte.Last.AirwayToNext = text;
                directToWptIfNoDuplicate(rte, starInfo.FirstWaypoint);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool tryParseSid(ManagedRoute rte, string text)
        {
            try
            {
                var sidManager = new SidHandler(origIcao);
                var sidInfo = sidManager.InfoForAnalysis(origRwy, text);

                rte.AddLastWaypoint(sidInfo.LastWaypoint, text, sidInfo.TotalDistance);
                sidLastWpt = sidInfo.LastWaypoint;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Trys to parse current node as airway (including, e.g. NATs). The return value indicates whether the parsing was successful.
        /// currentNode = Airway
        /// </summary>
        private bool tryParseAwy(ManagedRoute rte, NodeType prevNode, string text)
        {
            int lastWptIndex = WptList.FindByWaypoint(rte.Last.Waypoint);

            if (lastWptIndex < 0)
            {
                //last wpt is not connected to an airway
                return false;
            }

            foreach (var i in WptList.EdgesFrom(lastWptIndex))
            {
                if (WptList.GetEdge(i).value.Airway == text)
                {
                    rte.Last.AirwayToNext = text;
                    return true;
                }
            }
            return false;
        }

        //currentNode = Waypoint
        private bool tryParseWpt(ManagedRoute rte, NodeType prevNode, string text)
        {
            switch (prevNode)
            {
                case NodeType.Awy:
                    return tryFindWptOnAwy(rte, text);

                case NodeType.Orig:
                case NodeType.Wpt:
                    return tryAddWpt(rte, text);

                case NodeType.Sid:

                    if (sidLastWpt.ID != text)
                    {
                        return tryAddWpt(rte, text);
                    }
                    return true;

                default:
                    throw new ArgumentOutOfRangeException("Enum not supported.");
            }
        }

        private static bool tryFindWptOnAwy(ManagedRoute rte, string text)
        {
            int lastWptIndex = WptList.FindByWaypoint(rte.Last.Waypoint);
            string airway = rte.LastNode.Previous.Value.AirwayToNext;

            var wpts = new AirwayNodeFinder(lastWptIndex, airway, text,WptList).FindWaypoints();

            if (wpts == null)
            {
                return false;
            }

            addWptIfNoDuplicate(rte, wpts[0], airway,true);

            if (wpts.Count >= 2)
            {
                for (int i = 1; i < wpts.Count; i++)
                {
                    rte.AddLastWaypoint(wpts[i], airway, true);
                }
            }
            return true;
        }

        private static bool tryAddWpt(ManagedRoute rte, string ID)
        {
            Waypoint wpt = Utilities.FindWpt(ID, rte.Last.Waypoint);

            if (wpt == null)
            {
                return false;
            }
            else
            {
                directToWptIfNoDuplicate(rte, wpt);
                return true;
            }
        }

        private static void directToWptIfNoDuplicate(ManagedRoute rte, Waypoint wpt)
        {
            if (wpt.Equals(rte.Last.Waypoint) == false)
            {
                rte.AddLastWaypoint(wpt,"DCT", true);
            }
        }

        private static void addWptIfNoDuplicate(ManagedRoute rte, Waypoint wpt, string airway,bool AutoComputeDistance)
        {
            if (wpt.Equals(rte.Last.Waypoint) == false)
            {
                rte.AddLastWaypoint(wpt, airway, AutoComputeDistance);
            }
        }

        private static string exceptionMsg(string text, NodeType[] nodes)
        {
            var names = new List<string>();

            foreach (var i in nodes)
            {
                names.Add(nameInException(i));
            }

            names = names.Distinct().ToList();
            names.Remove("");

            string result = "\"" + text + "\" is not a valid ";

            if (names.Count == 1)
            {
                result += names[0];
            }
            else
            {
                for (int j = 0; j <= names.Count - 2; j++)
                {
                    result += names[j] + ", ";
                }

                result += "or " + names.Last();
            }

            result += ".";

            return result;
        }

        private static string nameInException(NodeType item)
        {
            switch (item)
            {
                case NodeType.Orig:
                case NodeType.Dest:
                    return "airport";

                case NodeType.Sid:
                    return "SID";

                case NodeType.Wpt:
                    return "waypoint";

                case NodeType.Awy:
                    return "airway";

                case NodeType.Star:
                    return "STAR";

                default:
                    throw new ArgumentOutOfRangeException("Enum not supported.");
            }
        }
    }
}

