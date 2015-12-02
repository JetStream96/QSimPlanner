using System;
using System.Collections.Generic;
using System.Linq;
using QSP.RouteFinding.Containers;
using static QSP.LibraryExtension.Arrays;
using QSP.Core;
using static QSP.RouteFinding.RouteFindingCore;
using static QSP.MathTools.MathTools;

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
            Start,
            Orig,
            Sid,
            Wpt,
            Awy,
            Star,
            Dest
        }

        private NodeType[] nextNode(NodeType currentNode)
        {
            switch (currentNode)
            {
                case NodeType.Start:
                    return new NodeType[] { NodeType.Orig, NodeType.Wpt, NodeType.Sid };

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
                    throw new ArgumentOutOfRangeException("Enum not supported.");
            }
        }

        public Route Parse()
        {
            Route result = new Route();
            string[] input = routeInput.ToUpper().Split(Delimiters, StringSplitOptions.RemoveEmptyEntries).RemoveElements("DCT");
            NodeType currentNode = NodeType.Start;

            sidLastWpt = null;

            //add orig rwy
            result.Waypoints.Add(new Waypoint(origIcao + origRwy, AirportList.RwyLatLon(origIcao, origRwy)));

            for (int i = 0; i < input.Length; i++)
            {
                NodeType[] nextNodeCandidate = nextNode(currentNode);

                if (nextNodeCandidate.Length == 0)
                {
                    break;
                }

                for (int j = 0; j <= nextNodeCandidate.Length - 1; j++)
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
                            throw new InvalidIdentifierException("\"" + input[i] + "\" is not a waypoint of airway " + result.Via.Last() + ".");
                        }
                        else
                        {
                            throw new InvalidIdentifierException(exceptionMsg(input[i], nextNodeCandidate) + WPT_RANGE_ERR);
                        }
                    }
                }
            }

            //add dest rwy
            addDest(result, new Waypoint(destIcao + destRwy, AirportList.RwyLatLon(destIcao, destRwy)));
            result.SetNat(NatsManager);

            return result;
        }


        private void addDest(Route rte, Waypoint dest)
        {
            if (rte.Waypoints.Count - 1 == rte.Via.Count)
            {
                rte.Via.Add("DCT");
                rte.TotalDis += GreatCircleDistance(rte.Waypoints.Last().LatLon, dest.LatLon);
            }

            rte.Waypoints.Add(dest);
        }

        /// <summary>
        /// This function modifies the route. Returns a bool indicates whether the parse was successful.
        /// </summary>
        /// <param name="rte"></param>
        /// <param name="prevNode"></param>
        /// <param name="currentNode"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private bool tryParse(Route rte, NodeType prevNode, NodeType currentNode, string text)
        {

            switch (currentNode)
            {

                case NodeType.Orig:

                    if (text == origIcao)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case NodeType.Dest:

                    if (text == destIcao)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

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

        private bool tryParseStar(Route rte, string text)
        {


            try
            {
                StarHandler starManager = new StarHandler(QspCore.AppSettings.NavDBLocation, destIcao);
                var starInfo = starManager.InfoForAnalysis(destRwy, text);

                rte.TotalDis += starInfo.Item1;
                addWptIfNoDuplicate(rte, starInfo.Item2);
                rte.Via.Add(text);

                return true;

            }
            catch
            {
                return false;
            }

        }

        private bool tryParseSid(Route rte, string text)
        {
            try
            {
                SidHandler sidManager = new SidHandler(QspCore.AppSettings.NavDBLocation, origIcao);
                var sidInfo = sidManager.InfoForAnalysis(origRwy, text);

                rte.TotalDis += sidInfo.Item1;
                rte.Via.Add(text);
                addWptIfNoDuplicate(rte, sidInfo.Item2);

                sidLastWpt = sidInfo.Item2;

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
        private bool tryParseAwy(Route rte, NodeType prevNode, string text)
        {
            int lastWptIndex = WptList.FindByWaypoint(rte.Waypoints.Last());

            if (lastWptIndex < 0)
            {
                //last wpt is not connected to an airway
                return false;
            }

            var neighbors = WptList.ElementAt(lastWptIndex).Neighbors;


            foreach (var i in neighbors)
            {
                if (i.Airway == text)
                {
                    rte.Via.Add(text);
                    return true;
                }

            }
            return false;
        }

        //currentNode = Waypoint
        private bool tryParseWpt(Route rte, NodeType prevNode, string text)
        {

            switch (prevNode)
            {

                case NodeType.Awy:
                    return tryFindWptOnAwy(rte, text);

                case NodeType.Start:
                case NodeType.Orig:
                case NodeType.Wpt:
                    return tryAddWpt(rte, text);

                case NodeType.Sid:

                    if (sidLastWpt.ID != text)
                    {
                        return tryAddWpt(rte, text);
                    }
                    else
                    {
                        return true;
                    }

                default:
                    throw new ArgumentOutOfRangeException("Enum not supported.");
            }

        }

        private bool tryFindWptOnAwy(Route rte, string text)
        {
            int lastWptIndex = WptList.FindByWaypoint(rte.Waypoints.Last());
            var wpts = new AirwayConnectionFinder(lastWptIndex, rte.Via.Last(), text).FindWaypoints();

            if (wpts == null)
            {
                return false;
            }

            string airway = rte.Via.Last();
            addWptIfNoDuplicate(ref rte, wpts[0], airway);

            if (wpts.Count >= 2)
            {

                for (int i = 1; i < wpts.Count; i++)
                {
                    rte.TotalDis += GreatCircleDistance(rte.Waypoints.Last().LatLon, wpts[i].LatLon);
                    rte.Via.Add(airway);
                    rte.Waypoints.Add(wpts[i]);

                }

            }

            return true;

        }

        private bool tryAddWpt(Route rte, string ID)
        {

            Waypoint wpt = Utilities.FindWpt(ID, rte.Waypoints.Last());

            if (wpt == null)
            {
                return false;
            }
            else
            {
                rte.TotalDis += GreatCircleDistance(rte.Waypoints.Last().LatLon, wpt.LatLon);
                addWptIfNoDuplicate(rte, wpt);
                return true;
            }

        }


        private void addWptIfNoDuplicate(Route rte, Waypoint wpt)
        {

            if (!wpt.Equals(rte.Waypoints.Last()))
            {
                if (rte.Waypoints.Count == rte.Via.Count + 1)
                {
                    rte.Via.Add("DCT");
                }

                rte.Waypoints.Add(wpt);

            }

        }


        private void addWptIfNoDuplicate(ref Route rte, Waypoint wpt, string airway)
        {

            if (!wpt.Equals(rte.Waypoints.Last()))
            {
                if (rte.Waypoints.Count == rte.Via.Count + 1)
                {
                    rte.Via.Add(airway);
                }

                rte.TotalDis += GreatCircleDistance(rte.Waypoints.Last().LatLon, wpt.LatLon);
                rte.Waypoints.Add(wpt);

            }

        }

        private string exceptionMsg(string text, NodeType[] nodes)
        {

            List<string> names = new List<string>();

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

        private string nameInException(NodeType item)
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

