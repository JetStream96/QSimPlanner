using System;
using System.Collections.Generic;
using QSP.RouteFinding.Routes;

namespace QSP.RouteFinding.FileExport.Providers
{
    public class Ifly747v2Provider
    {
        public static string GetExportText(Route route)
        {
            const string start = @"[RTE]
ORIGIN_AIRPORT=ABCD
DEST_AIRPORT=EFGH
";

            const string end = @"
[CDU]
CRZ_ALT=
COST_INDEX=
";

            const string template = @"
[RTE.{id}]
RouteName={route}
Name={wpt}
Latitude={lat}
Longitude={lon}
CrossThisPoint=0
Heading=0
Speed=0
Altitude=0
Frequency=
FrequencyID=
";

            if (route.Count < 2) throw new ArgumentException();
            var list = new List<string>();
            var current = route.First;
            var index = 0;
            while (current.Next != route.Last)
            {
                var next = current.Next;
                var airway = current.Value.AirwayToNext.Airway;
                var airwayStr = (index == 0 || airway == "DCT") ? "" : airway;
                var wpt = next.Value.Waypoint;
                var val = template.Replace("{id}", index.ToString())
                                  .Replace("{route}", airwayStr)
                                  .Replace("{wpt}", wpt.ID)
                                  .Replace("{lat}", wpt.Lat.ToString("0.000000"))
                                  .Replace("{lon}", wpt.Lon.ToString("0.000000"));

                list.Add(val);

                current = next;
                index++;
            }

            var startStr = start.Replace("ABCD", route.FirstWaypoint.ID.Substring(0, 4))
                                .Replace("EFGH", route.LastWaypoint.ID.Substring(0, 4));

            return string.Join("", startStr, string.Join("", list), end);
        }
    }
}
