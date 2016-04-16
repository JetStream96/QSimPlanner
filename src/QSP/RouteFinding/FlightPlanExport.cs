using System;
using System.Text;
using QSP.RouteFinding.Routes;

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
        public static string GeneratePmdgRteFile(ManagedRoute rte)
        {
            rte.Expand();

            int numWpts = rte.Count;
            //including dep/arr airports
            string icaoOrig = rte.First.Waypoint.ID.Substring(0, 4);
            string icaoDest = rte.Last.Waypoint.ID.Substring(0, 4);

            var result = new StringBuilder();
            appendOrigAirportPart(numWpts, icaoOrig, result);

            var node = rte.FirstNode.Next;

            while (node != rte.LastNode)
            {
                var airway = node.Value.AirwayToNext;
                var wpt = node.Value.Waypoint;

                if (airway == "DCT" || node.Next == rte.LastNode)
                {
                    airway = "DIRECT";
                }

                result.Append(wpt.ID + Environment.NewLine + "5" + Environment.NewLine + airway + Environment.NewLine + "1 ");
                result.AppendLine(latLonToPMDGRteFormat(wpt.Lat, wpt.Lon) + " 0" + Environment.NewLine + "0" + Environment.NewLine + "0" + Environment.NewLine + "0");
                result.AppendLine();
                node = node.Next;
            }

            appendDestAirportPart(icaoDest, result);

            return result.ToString();
        }

        /// <summary>
        /// Appends the ORIG airport part onto the StringBuilder.
        /// </summary>
        private static void appendOrigAirportPart(int numWpts, string icao, StringBuilder result)
        {
            var ad = RouteFindingCore.AirportList.Find(icao);

            result.AppendLine("Flight plan is built by QSimPlanner.");
            result.AppendLine();

            result.Append(numWpts + Environment.NewLine + Environment.NewLine);
            result.Append(icao + Environment.NewLine + "1" + Environment.NewLine + "DIRECT" + Environment.NewLine + "1 ");
            result.Append(latLonToPMDGRteFormat(ad.Lat, ad.Lon) + " " + ad.Elevation);
            result.AppendLine(Environment.NewLine + "-----" + Environment.NewLine + "1" + Environment.NewLine + "0" + Environment.NewLine + Environment.NewLine + "1" + Environment.NewLine + ad.Elevation);
            result.Append("-" + Environment.NewLine + "-1000000" + Environment.NewLine + "-1000000" + Environment.NewLine + Environment.NewLine);

        }

        /// <summary>
        /// Appends the DEST airport part onto the StringBuilder.
        /// </summary>
        private static void appendDestAirportPart(string icao, StringBuilder result)
        {
            var ad = RouteFindingCore.AirportList.Find(icao);

            result.Append(icao + Environment.NewLine + "1" + Environment.NewLine + "-" + Environment.NewLine + "1 ");
            result.Append(latLonToPMDGRteFormat(ad.Lat, ad.Lon) + " " + ad.Elevation);
            result.AppendLine(Environment.NewLine + "-----" + Environment.NewLine + "0" + Environment.NewLine + "0" + Environment.NewLine + Environment.NewLine + "1" + Environment.NewLine + ad.Elevation);
            result.Append("-" + Environment.NewLine + "-1000000" + Environment.NewLine + "-1000000" + Environment.NewLine + Environment.NewLine);

        }
    }
}
