using QSP.RouteFinding.Airports;
using QSP.RouteFinding.Routes;
using System.Text;

namespace QSP.RouteFinding.FileExport.Providers
{
    public class PmdgProvider : IExportProvider
    {
        private ManagedRoute route;
        private AirportManager airports;

        public PmdgProvider(ManagedRoute route, AirportManager airports)
        {
            this.route = route;
            this.airports = airports;
        }

        private static string pmdgLatLonFormat(double lat, double lon)
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
        public string GetExportText()
        {
            route.Expand();

            int numWpts = route.Count;

            // Including dep/arr airports
            string icaoOrig = route.First.Waypoint.ID.Substring(0, 4);
            string icaoDest = route.Last.Waypoint.ID.Substring(0, 4);

            var result = new StringBuilder();
            appendOrigAirportPart(numWpts, icaoOrig, result);

            var node = route.FirstNode.Next;

            while (node != route.LastNode)
            {
                var airway = node.Value.AirwayToNext;
                var wpt = node.Value.Waypoint;

                if (airway == "DCT" || node.Next == route.LastNode)
                {
                    airway = "DIRECT";
                }

                result.Append(wpt.ID + "\n5\n" + airway + "\n1 ");
                result.AppendLine(pmdgLatLonFormat(wpt.Lat, wpt.Lon) +
                    " 0\n0\n0\n0");
                result.AppendLine();
                node = node.Next;
            }

            appendDestAirportPart(icaoDest, result);
            return result.ToString();
        }

        // Appends the ORIG airport part onto the StringBuilder.
        private void appendOrigAirportPart(
            int numWpts,
            string icao,
            StringBuilder result)
        {
            var ad = airports.Find(icao);

            result.AppendLine("Flight plan is built by QSimPlanner.");
            result.AppendLine();

            result.Append(numWpts + "\n\n");
            result.Append(icao + "\n1\nDIRECT\n1 ");
            result.Append(pmdgLatLonFormat(ad.Lat, ad.Lon) +
                " " + ad.Elevation);
            result.AppendLine("\n-----\n1\n0\n\n1\n" + ad.Elevation);
            result.Append("-\n-1000000\n-1000000\n\n");
        }

        // Appends the DEST airport part onto the StringBuilder.        
        private void appendDestAirportPart(
            string icao,
            StringBuilder result)
        {
            var ad = airports.Find(icao);

            result.Append(icao + "\n1\n-\n1 ");
            result.Append(pmdgLatLonFormat(ad.Lat, ad.Lon) +
                " " + ad.Elevation);
            result.AppendLine("\n-----\n0\n0\n\n1\n" + ad.Elevation);
            result.Append("-\n-1000000\n-1000000\n\n");
        }
    }
}
