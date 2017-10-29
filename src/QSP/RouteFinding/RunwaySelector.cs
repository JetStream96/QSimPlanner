using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.Metar;

namespace QSP.RouteFinding
{
    public static class RunwaySelector
    {
        // @NoThrow
        // error is not null if and only if an error occurred.
        public static (string runway, string error) SelectRunway(AirportManager airportManager,
            MetarCache cache, string icao)
        {
            var airport = airportManager[icao];
            if (airport == null) return (null, "Airport not found.");
            var runways = airport.Rwys;
            if (runways.Count == 0) return (null, "No runway is available.");
            var groups = GroupIntoPairs(runways).ToList();
            if (groups.Count == 0) return (null, "No runway is available.");
            // TODO:
        }

        public static IEnumerable<IEnumerable<RwyData>> GroupIntoPairs(IEnumerable<RwyData> rwys)
        {
            var rwyDict = rwys.ToDictionary(i => i.RwyIdent);
            var dict = new Dictionary<string, List<RwyData>>();

            foreach (var kv in rwyDict)
            {
                var runway = kv.Value;
                var id = kv.Key;
                var opposite = AviationTools.RwyIdentConversion.RwyIdentOppositeDir(id);
                if (opposite == null) continue;

                if (rwyDict.TryGetValue(opposite, out var val))
                {
                    dict.Add(id, new List<RwyData>() { runway, val });
                }
            }

            return dict.Values;
        }
    }
}
