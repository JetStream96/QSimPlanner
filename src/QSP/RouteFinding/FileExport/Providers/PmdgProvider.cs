using QSP.RouteFinding.Airports;
using System.Text;

namespace QSP.RouteFinding.FileExport.Providers
{
    public static class PmdgProvider
    {
        private static string PmdgLatLonFormat(double lat, double lon)
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
        /// Returns a string represents the PMDG .rte file. 
        /// SIDs, STARs are not included.
        /// </summary>
        public static string GetExportText(ExportInput input)
        {
            var route = input.Route;
            var airports = input.Airports;
            int numWpts = route.Count;

            // Including dep/arr airports
            string icaoOrig = route.FirstWaypoint.ID.Substring(0, 4);
            string icaoDest = route.LastWaypoint.ID.Substring(0, 4);

            var result = new StringBuilder();
            AppendOrigAirportPart(numWpts, icaoOrig, result, airports);

            var node = route.First.Next;

            while (node != route.Last)
            {
                var airway = node.Value.AirwayToNext.Airway;
                var wpt = node.Value.Waypoint;

                if (airway == "DCT" || node.Next == route.Last)
                {
                    airway = "DIRECT";
                }

                result.Append(wpt.ID.FormatWaypointId() + "\n5\n" + airway + "\n1 ");
                result.AppendLine(PmdgLatLonFormat(wpt.Lat, wpt.Lon) +
                    " 0\n0\n0\n0");
                result.AppendLine();
                node = node.Next;
            }

            AppendDestAirportPart(icaoDest, result, airports);
            return result.ToString();
        }

        // Appends the ORIG airport part onto the StringBuilder.
        private static void AppendOrigAirportPart(int numWpts, string icao,
            StringBuilder result, AirportManager airports)
        {
            var ad = airports[icao];

            result.AppendLine("Flight plan is built by QSimPlanner.");
            result.AppendLine();

            result.Append(numWpts + "\n\n");
            result.Append(icao + "\n1\nDIRECT\n1 ");
            result.Append(PmdgLatLonFormat(ad.Lat, ad.Lon) +
                " " + ad.Elevation);
            result.AppendLine("\n-----\n1\n0\n\n1\n" + ad.Elevation);
            result.Append("-\n-1000000\n-1000000\n\n");
        }

        // Appends the DEST airport part onto the StringBuilder.        
        private static void AppendDestAirportPart(string icao,
            StringBuilder result, AirportManager airports)
        {
            var ad = airports[icao];

            result.Append(icao + "\n1\n-\n1 ");
            result.Append(PmdgLatLonFormat(ad.Lat, ad.Lon) +
                " " + ad.Elevation);
            result.AppendLine("\n-----\n0\n0\n\n1\n" + ad.Elevation);
            result.Append("-\n-1000000\n-1000000\n\n");
        }
    }
}
