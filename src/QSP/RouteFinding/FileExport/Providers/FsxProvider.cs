using QSP.LibraryExtension;
using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Containers;
using QSP.RouteFinding.Data.Interfaces;
using QSP.RouteFinding.Routes;
using System;
using System.Linq;
using System.Xml.Linq;
using static QSP.AviationTools.Coordinates.FormatDegreeMinuteSecond;

namespace QSP.RouteFinding.FileExport.Providers
{
    public class FsxProvider : IExportProvider
    {
        private Route route;
        private AirportManager airports;

        public FsxProvider(Route route, AirportManager airports)
        {
            if (route.Count < 2) throw new ArgumentException();

            this.route = route;
            this.airports = airports;
        }

        public string GetExportText()
        {
            var version = new XElement("AppVersion",
                new XElement("AppVersionMajor", "10"),
                new XElement("AppVersionBuild", "61637"));

            var orig = route.FirstWaypoint;
            var origId = orig.ID;
            var origIcao = origId.Substring(0, 4);
            var origRwy = origId.Substring(4);
            var origAirport = airports[origIcao];
            var origLatLonAlt = LatLonAlt(orig, origAirport.Elevation);

            var dest = route.LastWaypoint;
            var destId = dest.ID;
            var destIcao = destId.Substring(0, 4);
            var destRwy = destId.Substring(4);
            var destAirport = airports[destIcao];
            var destLatLonAlt = LatLonAlt(dest, destAirport.Elevation);

            var origNode = new XElement("ATCWaypoint",
                new XElement("ATCWaypointType", "Airport"),
                new XElement("WorldPosition", origLatLonAlt),
                new XElement("ICAO",
                    new XElement("ICAOIdent", origIcao)));
            origNode.SetAttributeValue("id", origIcao);

            var destNode = new XElement("ATCWaypoint",
                new XElement("ATCWaypointType", "Airport"),
                new XElement("WorldPosition", destLatLonAlt),
                new XElement("ICAO",
                    new XElement("ICAOIdent", destIcao)));
            destNode.SetAttributeValue("id", destIcao);

            var waypoints = route.Select(n => n.Waypoint).ToArray();
            var wptNodes = waypoints.WithoutFirstAndLast().Select(GetWaypointNode);

            var flightPlanChild = new XElement[]
            {
                new XElement("Title", origIcao + " to " + destIcao),
                new XElement("FPType", "IFR"),
                new XElement("RouteType", "HighAlt"),
                new XElement("CruisingAlt", "10000"),
                new XElement("DepartureID", origIcao),
                new XElement("DepartureLLA", origLatLonAlt),
                new XElement("DestinationID", destIcao),
                new XElement("DestinationLLA", destLatLonAlt),
                new XElement("Descr", origIcao + ", " + destIcao),
                new XElement("DeparturePosition", origRwy),
                new XElement("DepartureName", origAirport.Name),
                new XElement("DestinationName", destAirport.Name),
                version
            };

            var flightPlanNode = new XElement("FlightPlan.FlightPlan",
                flightPlanChild, origNode, wptNodes, destNode);

            var root = new XElement("SimBase.Document",
                new XElement("Descr", "AceXML Document"),
                flightPlanNode);

            root.SetAttributeValue("Type", "AceXML");
            root.SetAttributeValue("version", "1,0");

            var doc = new XDocument(root);
            return doc.ToString();
        }

        private static XElement GetWaypointNode(Waypoint wpt)
        {
            var node = new XElement("ATCWaypoint",
                new XElement("ATCWaypointType", "Intersection"),
                new XElement("WorldPosition", LatLonAlt(wpt, 0.0)),
                new XElement("ICAO",
                    new XElement("ICAOIdent", wpt.ID)));

            node.SetAttributeValue("id", wpt.ID);
            return node;
        }

        public static string LatLonAlt(ICoordinate latLon, double altitudeFt)
        {
            var format = "F2";
            var lat = LatToDegreeMinuteSecondFormat(latLon.Lat, format);
            var lon = LonToDegreeMinuteSecondFormat(latLon.Lon, format);
            var alt = altitudeFt.ToString(format);

            if (alt[0] != '-') alt = '+' + alt;
            alt = alt[0] + alt.Substring(1).PadLeft(9, '0');

            return lat + ',' + lon + ',' + alt;
        }
    }
}
