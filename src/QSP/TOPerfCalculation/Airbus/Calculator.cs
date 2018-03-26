using QSP.TOPerfCalculation.Airbus.DataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.TOPerfCalculation.Boeing;
using QSP.AviationTools;
using static QSP.MathTools.Angles;
using static QSP.LibraryExtension.Types;
using QSP.MathTools;
using QSP.MathTools.Tables;
using QSP.MathTools.Interpolation;

namespace QSP.TOPerfCalculation.Airbus
{
    public static class Calculator
    {
        public static TOReport TakeOffReport(
            AirbusPerfTable table, Parameters para, double tempIncrement = 1.0)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Computes the required takeoff distance.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static double TakeOffDistance(AirbusPerfTable t, Parameters p)
        {
            var windCorrectedFt = p.RwyLengthMeter * Constants.MeterFtRatio
                + WindCorrectionFt(t, p);
            var slopeCorrectedFt = windCorrectedFt + SlopeCorrectionFt(t, p, windCorrectedFt);
            var tables = GetTables(t, p);

            if (tables.Count == 0) throw new Exception("No data for selected flaps setting.");
            var pressAlt = ConversionTools.PressureAltitudeFt(p.RwyElevationFt, p.QNH);
            var inverseTables = tables.Select(x => GetInverseTable(x, pressAlt));
            var distances = inverseTables.Select(
                x => x.ValueAt(p.WeightKg * 0.001 * Constants.KgLbRatio)).ToArray();
            if (distances.Length == 1) return distances[0];
            return Interpolate1D.Interpolate(
                tables.Select(x => x.IsaOffset).ToArray(),
                distances,
                IsaOffset(p));
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

        // The table is for limit weight. This method constructs a table of 
        // takeoff distance. (x: weight 1000 LB, f: runway length ft)
        private static Table1D GetInverseTable(TableDataNode n, double pressAlt)
        {
            var t = n.Table;
            var len = t.x;
            var weight = len.Select(i => t.ValueAt(i, pressAlt));
            return new Table1D(weight.ToArray(), len);
        }
    }

}
