using QSP.AviationTools;
using QSP.MathTools;
using QSP.MathTools.Interpolation;
using QSP.MathTools.Tables;
using QSP.TOPerfCalculation.Airbus.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using static QSP.MathTools.Angles;

namespace QSP.TOPerfCalculation.Airbus
{
    public static class Calculator
    {
        public static TOReport TakeOffReport(
            AirbusPerfTable table, Parameters para, double tempIncrement = 1.0)
        {
            throw new NotImplementedException();
        }

        public enum Error
        {
            None,
            NoDataForSelectedFlaps
        }

        /// <summary>
        /// Computes the required takeoff distance.
        /// </summary>
        public static (Error e, double dis) TakeOffDistanceMeter(
            AirbusPerfTable t, Parameters p)
        {
            var tables = GetTables(t, p);

            if (tables.Count == 0) return (Error.NoDataForSelectedFlaps, 0.0);
            var pressAlt = ConversionTools.PressureAltitudeFt(p.RwyElevationFt, p.QNH);
            var inverseTables = tables.Select(x => GetInverseTable(x, pressAlt, t, p));
            var distancesMeter = inverseTables.Select(x =>
                x.ValueAt(p.WeightKg * 0.001 * Constants.KgLbRatio) * Constants.FtMeterRatio)
                .ToArray();
            if (distancesMeter.Length == 1) return (Error.None, distancesMeter[0]);
            return (Error.None, Interpolate1D.Interpolate(
                tables.Select(x => x.IsaOffset).ToArray(),
                distancesMeter,
                IsaOffset(p)));
        }

        // Use the length of first argument instead of the one in Parameters.
        private static double SlopeAndWindCorrectedLengthFt(double lengthFt,
            AirbusPerfTable t, Parameters p)
        {
            var windCorrectedFt = lengthFt + WindCorrectionFt(t, p);
            return windCorrectedFt + SlopeCorrectionFt(t, p, windCorrectedFt);
        }

        private static double BleedAirCorrection1000LB(AirbusPerfTable t, Parameters p)
        {
            if (p.PacksOn) return t.PacksOnCorrection;
            if (p.AntiIce == 2) return t.AllAICorrection;
            if (p.AntiIce == 1) return t.EngineAICorrection;
            return 0.0;
        }

        private static double WetCorrection1000LB(double lengthFt,
            AirbusPerfTable t, Parameters p)
        {
            if (!p.SurfaceWet) return 0.0;
            return t.WetCorrectionTable.ValueAt(lengthFt);
        }

        private static double WindCorrectionFt(AirbusPerfTable t, Parameters p)
        {
            var lenft = p.RwyLengthMeter * Constants.MeterFtRatio;
            var headwind = p.WindSpeedKnots *
               Math.Cos(ToRadian(p.RwyHeading - p.WindHeading));

            return (headwind >= 0 ?
                t.HeadwindCorrectionTable.ValueAt(lenft) :
                t.TailwindCorrectionTable.ValueAt(lenft)) * headwind;
        }

        private static double SlopeCorrectionFt(AirbusPerfTable t, Parameters p,
            double windCorrectedLengthFt)
        {
            var len = windCorrectedLengthFt;
            var s = p.RwySlopePercent;
            return (s >= 0 ?
                t.UphillCorrectionTable.ValueAt(len) :
                t.DownHillCorrectionTable.ValueAt(len)) * s;
        }

        private static double IsaOffset(Parameters p) =>
            p.OatCelsius - ConversionTools.IsaTemp(p.RwyElevationFt);

        // Returns best matching tables, returning list can have:
        // 0 element if no matching flaps, or
        // 1 element if only 1 table matches the flaps setting, or
        // 2 elements if more than 1 table match the flaps, these two tables are
        // the ones most suitable for ISA offset interpolation.
        private static List<TableDataNode> GetTables(AirbusPerfTable t, Parameters p)
        {
            var sameFlaps = t.Tables.Where(x => x.Flaps == p.Flaps).ToList();
            if (sameFlaps.Count <= 1) return sameFlaps;
            var ordered = sameFlaps.OrderBy(x => x.IsaOffset).ToList();
            var isaOffset = IsaOffset(p);
            var skip = ordered.Where(x => isaOffset > x.IsaOffset).Count() - 1;
            var actualSkip = Numbers.LimitToRange(skip, 0, ordered.Count - 2);
            return ordered.Skip(actualSkip).Take(2).ToList();
        }

        private static double WetAndBleedAirCorrection1000LB(double lengthFt,
            AirbusPerfTable t, Parameters p) =>
            WetCorrection1000LB(lengthFt, t, p) + BleedAirCorrection1000LB(t, p);

        // The table is for limit weight. This method constructs a table of 
        // takeoff distance. (x: weight 1000 LB, f: runway length ft)
        // Wet runway and bleed air corrections are applied here.
        private static Table1D GetInverseTable(TableDataNode n, double pressAlt,
            AirbusPerfTable t, Parameters p)
        {
            var table = n.Table;
            var len = table.y.Select(i => SlopeAndWindCorrectedLengthFt(i, t, p));
            var weight = len.Select(i =>
                table.ValueAt(pressAlt, i) - WetAndBleedAirCorrection1000LB(i, t, p));
            return new Table1D(weight.ToArray(), len.ToArray());
        }
    }
}
