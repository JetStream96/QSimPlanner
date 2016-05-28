using System.Text;
using QSP.RouteFinding.Routes;
using QSP.RouteFinding.Airports;

namespace QSP.RouteFinding
{
    public static class FlightPlanExport
    {
        private static string latLonToPMDGRteFormat(Vector2D latlon)
        {
            return latLonToPMDGRteFormat(latlon.x, latlon.y);
        }

        private static string latLonToPMDGRteFormat(double lat, double lon)
        {
            string s = null;

            if (lat > 0.0)
            {
                s = "N " + lat;
            }
            else
            {
                s = "S " + (-lat);
            }

            if (lon > 0.0)
            {
                s += " E " + lon;
            }
            else
            {
                s += " W " + (-lon);
            }
            return s;
        }

        /// <summary>
        /// Returns a string represents the PMDG .rte file. SIDs, STARs are not included.
        /// </summary>
        /// <param name="rte"></param>
        public static string GeneratePmdgRteFile(
            ManagedRoute rte, AirportManager airportList)
        {
            rte.Expand();

            int numWpts = rte.Count;
            //including dep/arr airports
            string icaoOrig = rte.First.Waypoint.ID.Substring(0, 4);
            string icaoDest = rte.Last.Waypoint.ID.Substring(0, 4);

            var result = new StringBuilder();
            appendOrigAirportPart(numWpts, icaoOrig, airportList, result);

            var node = rte.FirstNode.Next;

            while (node != rte.LastNode)
            {
                var airway = node.Value.AirwayToNext;
                var wpt = node.Value.Waypoint;

                if (airway == "DCT" || node.Next == rte.LastNode)
                {
                    airway = "DIRECT";
                }

                result.Append(wpt.ID + "\n" + "5" + "\n" + airway + "\n" + "1 ");
                result.AppendLine(latLonToPMDGRteFormat(wpt.Lat, wpt.Lon) +
                    " 0\n0\n0\n0");
                result.AppendLine();
                node = node.Next;
            }

            appendDestAirportPart(icaoDest, airportList, result);

            return result.ToString();
        }

        /// <summary>
        /// Appends the ORIG airport part onto the StringBuilder.
        /// </summary>
        private static void appendOrigAirportPart(
            int numWpts,
            string icao,
            AirportManager airportList,
            StringBuilder result)
        {
            var ad = airportList.Find(icao);

            result.AppendLine("Flight plan is built by QSimPlanner.");
            result.AppendLine();

            result.Append(numWpts + "\n\n");
            result.Append(icao + "\n1\nDIRECT\n1 ");
            result.Append(latLonToPMDGRteFormat(ad.Lat, ad.Lon) + " " + ad.Elevation);
            result.AppendLine("\n" + "-----" + "\n1\n0\n\n1\n" + ad.Elevation);
            result.Append("-" + "\n" + "-1000000" + "\n" + "-1000000" + "\n\n");

        }

        /// <summary>
        /// Appends the DEST airport part onto the StringBuilder.
        /// </summary>
        private static void appendDestAirportPart(
            string icao,
            AirportManager airportList,
            StringBuilder result)
        {
            var ad = airportList.Find(icao);

            result.Append(icao + "\n1\n-\n1 ");
            result.Append(latLonToPMDGRteFormat(ad.Lat, ad.Lon) + " " + ad.Elevation);
            result.AppendLine("\n-----\n0\n0\n\n1\n" + ad.Elevation);
            result.Append("-\n" + "-1000000" + "\n" + "-1000000" + "\n" + "\n");

        }
    }
}
