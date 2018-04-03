using QSP.AviationTools;
using QSP.Common;
using QSP.Utilities;
using System;
using System.Linq;
using static QSP.MathTools.Numbers;

namespace QSP.LandingPerfCalculation.Airbus
{
    // SurfaceCondition: ['Dry', 'Wet']
    // Reversers: ['None', 'All']
    //
    public static class Calculator
    {
        /// <summary>
        /// Overrides the brake settings of CalculatorData.
        /// </summary>
        /// <exception cref="RunwayTooShortException"></exception>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public static LandingReport LandingReport(CalculatorData d)
        {
            var p = d.Parameters;
            var t = d.Table;

            var all = t.AllBrakes().Select(b =>
            {
                return ExceptionHelpers.DefaultIfThrows(() =>
                {
                    var dis = LandingDistanceMeter(d, null, b);
                    return new ReportRow()
                    {
                        BrakeSetting = b,
                        RequiredDistanceMeter = RoundToInt(dis),
                        RemainDistanceMeter = RoundToInt(p.RwyLengthMeter - dis)
                    };
                }, null);
            }).ToList();

            var selected = all[p.BrakeIndex];
            if (selected == null) throw new Exception("Unexpected error occurred.");
            if (selected.RemainDistanceMeter < 0) throw new RunwayTooShortException();

            return new LandingReport()
            {
                SelectedBrake = selected,
                AllBrakes = all
            };
        }

        /// <summary>
        /// Flaps and brake settings can be overriden.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static double LandingDistanceMeter(CalculatorData d,
            string flaps = null, string brake = null)
        {
            var p = d.Parameters;
            var t = d.Table;
            if (flaps == null) flaps = t.AllFlaps()[p.FlapsIndex];
            if (brake == null) brake = t.AllBrakes()[p.BrakeIndex];

            // Matching table
            var m = t.Entries.Where(x => x.Flaps == flaps && x.Autobrake == brake)
                .First();

            var weight1000LB = p.WeightKG / 1000 * Constants.KgLbRatio;
            var (table, elevation, head, tail, reverser) = p.SurfaceCondition == 0
                ? (m.Dry, m.ElevationDry, m.HeadwindDry, m.TailwindDry, m.BothReversersDry)
                : (m.Wet, m.ElevationWet, m.HeadwindWet, m.TailwindWet, m.BothReversersWet);

            var len = table.ValueAt(weight1000LB);
            var alt = ConversionTools.PressureAltitudeFt(p.ElevationFT, p.QNH);
            len += alt / 1000 * elevation;
            len += p.HeadwindKts >= 0 ?
                (-head * p.HeadwindKts / 10) :
                (tail * -p.HeadwindKts / 10);
            len -= p.Reverser == 1 ? reverser : 0;
            len *= 1 + (p.AppSpeedIncrease / 5) * (m.Speed5Knots / 100);
            return len * Constants.FtMeterRatio;
        }
    }
}
