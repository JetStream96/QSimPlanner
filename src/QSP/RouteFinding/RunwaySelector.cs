using QSP.AircraftProfiles.Configs;
using QSP.AviationTools;
using QSP.LandingPerfCalculation.Boeing;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.Metar;
using QSP.RouteFinding.Airports;
using QSP.TOPerfCalculation.Boeing;
using QSP.TOPerfCalculation.Boeing.PerfData;
using QSP.UI.UserControls.TakeoffLanding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Landing = QSP.LandingPerfCalculation;
using LandingPerf = QSP.LandingPerfCalculation.Boeing.PerfData;
using TO = QSP.TOPerfCalculation;
using TOPerf = QSP.TOPerfCalculation.Boeing.PerfData;

namespace QSP.RouteFinding
{
    public class RunwaySelector
    {
        private AirportManager airportManager;
        private MetarCache cache;
        private AcConfigManager aircrafts;
        private List<TO.PerfTable> takeoffTables;
        private List<Landing.PerfTable> landingTables;

        public RunwaySelector(
            AirportManager airportManager,
            MetarCache cache,
            AcConfigManager aircrafts,
            List<TO.PerfTable> takeoffTables,
            List<Landing.PerfTable> landingTables)
        {
            this.airportManager = airportManager;
            this.cache = cache;
            this.aircrafts = aircrafts;
            this.takeoffTables = takeoffTables;
            this.landingTables = landingTables;
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
            var (success, runway, windHeading, windSpeed, rwyHeading, qnh) =
                GetParameters(airport, rwy, item);
            if (!success) return null;

            var (ac, perf) = TO.FindTable.Find(takeoffTables, aircrafts, aircraft.Registration);
            if (ac == null || perf == null) return null;
            var perfItem = (TOPerf.BoeingPerfTable)(perf.Item);
            var para = new TOParameters(
                runway.LengthFt * Constants.FtMeterRatio,
                runway.ElevationFt,
                rwyHeading,
                airport.GetSlopePercent(rwy) ?? 0.0,
                windHeading,
                windSpeed,
                item.Temp.HasValue ? item.Temp.Value : 15,
                qnh,
                item.PrecipitationExists,
                aircraft.TakeOffWeightKg,
                0,
                AntiIceOption.Off,
                true,
                0);

            try
            {
                return new TOCalculator(perfItem, para).TakeoffDistanceMeter();
            }
            catch
            {
                return null;
            }
        }

        // @NoThrow
        // Returns null if failed to calculate.
        public double? LandingDistanceMeter(Airport airport, string rwy,
            MetarCacheItem item, AircraftRequest aircraft)
        {
            var (success, runway, windHeading, windSpeed, rwyHeading, qnh) =
                GetParameters(airport, rwy, item);
            if (!success) return null;

            var (ac, perf) = Landing.FindTable.Find(landingTables, aircrafts, aircraft.Registration);
            if (ac == null || perf == null) return null;
            var perfItem = (LandingPerf.BoeingPerfTable)(perf.Item);
            var para = new LandingParameters(
                aircraft.LandingWeightKg,
                runway.LengthFt * Constants.FtMeterRatio,
                runway.ElevationFt,
                ConversionTools.HeadwindComponent(rwyHeading, windHeading, windSpeed),
                airport.GetSlopePercent(rwy) ?? 0.0,
                item.Temp.HasValue ? item.Temp.Value : 15,
                qnh,
                5,
                ReverserOption.Both,
                item.PrecipitationExists ? SurfaceCondition.Good : SurfaceCondition.Dry,
                0,
                0);

            try
            {
                return new LandingCalculator(perfItem, para).DistanceRequiredMeter();
            }
            catch
            {
                return null;
            }
        }

        private static (bool success, IRwyData runway, double windHeading, double windSpeed,
            double rwyHeading, double QNH)
            GetParameters(Airport airport, string rwy, MetarCacheItem item)
        {
            var runway = airport.FindRwy(rwy);
            if (runway == null) return (false, null, 0, 0, 0, 0);

            var w = item.Wind;
            var (windHeading, windSpeed) = w.HasValue ? (w.Value.Direction, w.Value.Speed) : (0, 0);

            double GetQNH()
            {
                var press = item.Pressure;
                if (!press.hasValue) return 1013.2;
                return press.value *
                (press.unit == Utilities.Units.PressureUnit.inHg ? 1013.2 / 29.92 : 1);
            }

            var rwyHeading = ConversionTools.ParseHeading(runway.Heading) ?? 0;
            return (true, runway, windHeading, windSpeed, rwyHeading, GetQNH());
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
