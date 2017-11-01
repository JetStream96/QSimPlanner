using QSP.Utilities;
using QSP.RouteFinding.Airports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.Metar;
using QSP.UI.UserControls.TakeoffLanding.Common;
using QSP.TOPerfCalculation.Boeing;
using QSP.AviationTools;
using QSP.AircraftProfiles.Configs;
using QSP.TOPerfCalculation;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace QSP.RouteFinding
{
    public class RunwaySelector
    {
        private AirportManager airportManager;
        private MetarCache cache;
        private AcConfigManager aircrafts;
        private List<PerfTable> tables;

        public RunwaySelector(AirportManager airportManager, MetarCache cache)
        {
            this.airportManager = airportManager;
            this.cache = cache;
        }

        // @NoThrow
        // error is not null if and only if an error occurred.
        public (string runway, string error) SelectRunway(string icao)
        {
            var airport = airportManager[icao];
            if (airport == null) return (null, "Airport not found.");
            var runways = airport.Rwys;
            if (runways.Count == 0) return (null, "No runway is available.");
            var groups = GroupIntoPairs(runways).ToList();
            if (groups.Count == 0) return (null, "No runway is available.");
            // TODO:
            throw new Exception();
        }

        // @NoThrow
        // Returns null if failed to calculate.
        public double? TakeOffDistanceMeter(Airport airport, string rwy,
            MetarCacheItem item, AircraftRequest aircraft)
        {
            var runway = airport.FindRwy(rwy);
            if (runway == null) return null;

            var w = item.Wind;
            var (windHeading, windSpeed) = w.HasValue ? (w.Value.Direction, w.Value.Speed) : (0, 0);

            double GetQNH()
            {
                var press = item.Pressure;
                if (!press.hasValue) return 1013.2;
                return press.value *
                (press.unit == Utilities.Units.PressureUnit.inHg ? 1013.2 / 29.92 : 1);
            }

            var (ac, perf) = FindTable.Find(tables, aircrafts, aircraft.Registration);
            if (ac == null || perf == null) return null;
            var perfItem = (BoeingPerfTable)(perf.Item);
            var para = new TOParameters(
                runway.LengthFt * Constants.FtMeterRatio,
                runway.ElevationFt,
                double.Parse( runway.Heading),
                airport.GetSlopePercent(rwy) ?? 0.0,
                windHeading,
                windSpeed,
                item.Temp.HasValue ? item.Temp.Value : 15,
                GetQNH(),
                item.PrecipitationExists,
                aircraft.TakeOffWeightKg,
                0,
                AntiIceOption.Off,
                true,
                0);

            var calc = new TOCalculator(perfItem, para);

            try
            {
                return calc.TakeoffDistanceMeter();
            }
            catch
            {
                return null;
            }
        }

        public static IEnumerable<IReadOnlyList<IRwyData>> GroupIntoPairs(
            IEnumerable<IRwyData> rwys)
        {
            var rwyDict = rwys.ToDictionary(i => i.RwyIdent);
            var dict = new Dictionary<string, List<IRwyData>>();

            foreach (var kv in rwyDict)
            {
                var runway = kv.Value;
                var id = kv.Key;
                var opposite = RwyIdentConversion.RwyIdentOppositeDir(id);
                if (opposite == null || dict.ContainsKey(id) || dict.ContainsKey(opposite))
                {
                    continue;
                }

                if (rwyDict.TryGetValue(opposite, out var val))
                {
                    dict.Add(id, new List<IRwyData>() { runway, val });
                }
            }

            return dict.Values;
        }
    }
}
