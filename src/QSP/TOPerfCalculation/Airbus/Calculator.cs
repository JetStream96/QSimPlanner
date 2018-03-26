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

namespace QSP.TOPerfCalculation.Airbus
{
    public static class Calculator
    {
        public static TOReport TakeOffReport(
            AirbusPerfTable table, Parameters para, double tempIncrement = 1.0)
        {
            throw new NotImplementedException();
        }

        public static double TakeOffDistance(AirbusPerfTable table, Parameters para)
        {
            double WindCorrectionFt()
            {
                var lenft = para.RwyLengthMeter * Constants.MeterFtRatio;
                var headwind = para.WindSpeedKnots *
                   Math.Cos(ToRadian(para.RwyHeading - para.WindHeading));

                return (headwind >= 0 ?
                    table.HeadwindCorrectionTable.ValueAt(lenft) :
                    table.TailwindCorrectionTable.ValueAt(lenft)) * headwind;
            }

            double SlopeCorrectionFt(double windCorrectedLengthFt)
            {
                var len = windCorrectedLengthFt;
                var s = para.RwySlopePercent;
                return (s >= 0 ?
                    table.UphillCorrectionTable.ValueAt(len) :
                    table.DownHillCorrectionTable.ValueAt(len)) * s;
            }

            // Returns best matching tables, returning list can have:
            // 0 element if no matching flaps, or
            // 1 element if only 1 table matches the flaps setting, or
            // 2 elements if more than 1 table match the flaps, these two tables are
            // the ones most suitable for ISA offset interpolation.
            List<TableDataNode> GetTables()
            {
                var sameFlaps = table.Tables.Where(t => t.Flaps == para.Flaps).ToList();
                if (sameFlaps.Count <= 1) return sameFlaps;
                var ordered = sameFlaps.OrderBy(t => t.IsaOffset).ToList();
                var isaOffset = para.OatCelsius - ConversionTools.IsaTemp(para.RwyElevationFt);
                var skip = ordered.Where(t => isaOffset > t.IsaOffset).Count() - 1;
                var actualSkip = Numbers.LimitToRange(skip, 0, ordered.Count - 2);
                return ordered.Skip(actualSkip).Take(2).ToList();
            }

            var windCorrectedFt = para.RwyLengthMeter * Constants.MeterFtRatio + WindCorrectionFt();
            var slopeCorrectedFt = windCorrectedFt + SlopeCorrectionFt(windCorrectedFt);
            var tables = GetTables();
            throw new NotImplementedException();
        }


    }

}
