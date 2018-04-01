using QSP.Common;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using System.Collections.Generic;
using System.Linq;
using static QSP.MathTools.Numbers;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class LandingReportGenerator
    {
        private BoeingPerfTable perfTable;
        private LandingParameters para;
        private LandingCalculator calc;

        public LandingReportGenerator(BoeingPerfTable perfTable, LandingParameters para)
        {
            this.perfTable = perfTable;
            this.para = para;
            calc = new LandingCalculator(perfTable, para);
        }

        /// <exception cref="RunwayTooShortException"></exception>
        public LandingReport GetReport()
        {
            return new LandingReport()
            {
                SelectedBrake = GetMainResult(),
                AllBrakes = GetOtherResults()
            };
        }

        private ReportRow GetMainResult()
        {
            var brkList = perfTable.BrakesAvailable(para.SurfaceCondition);
            double disReqMeter = calc.DistanceRequiredMeter();
            double disRemainMeter = para.RwyLengthMeter - disReqMeter;

            if (disRemainMeter < 0) throw new RunwayTooShortException();
            return new ReportRow()
            {
                BrakeSetting = brkList[para.BrakeIndex],
                RequiredDistanceMeter = RoundToInt(disReqMeter),
                RemainDistanceMeter = RoundToInt(disRemainMeter)
            };
        }

        private List<ReportRow> GetOtherResults()
        {
            var brakes = perfTable.BrakesAvailable(para.SurfaceCondition);
            return brakes.Select((b, i) =>
            {
                double disReq = RoundToInt(calc.DistanceRequiredMeter(i));
                double disRemain = para.RwyLengthMeter - disReq;
                return new ReportRow()
                {
                    BrakeSetting = b,
                    RequiredDistanceMeter = RoundToInt(disReq),
                    RemainDistanceMeter = RoundToInt(disRemain)
                };
            }).Where(r => r.RemainDistanceMeter >= 0).ToList();
        }
    }
}
