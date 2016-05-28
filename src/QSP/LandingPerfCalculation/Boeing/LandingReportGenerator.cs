using QSP.Common;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using static QSP.MathTools.Doubles;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class LandingReportGenerator
    {
        private BoeingPerfTable perfTable;
        private LandingParameters para;
        private LandingCalculator calc;

        public LandingReportGenerator(BoeingPerfTable perfTable,
                                      LandingParameters para)
        {
            this.perfTable = perfTable;
            this.para = para;
            calc = new LandingCalculator(perfTable, para);
        }

        /// <exception cref="RunwayTooShortException"></exception>
        public LandingReport GetReport()
        {
            var report = new LandingReport();

            //compute the user input           
            validateMainResult(report);

            //compute all possible brake settings
            validateOtherResults(report);

            return report;
        }

        private void validateMainResult(LandingReport report)
        {
            var brkList = perfTable.BrakesAvailable(para.SurfaceCondition);
            double disReqMeter = calc.DistanceRequiredMeter();
            double disRemainMeter = para.RwyLengthMeter - disReqMeter;

            if (disRemainMeter >= 0)
            {
                report.SetSelectedBrakesResult(
                    brkList[para.BrakeIndex],
                    RoundToInt(disReqMeter),
                    RoundToInt(disRemainMeter));
            }
            else
            {
                throw new RunwayTooShortException();
            }
        }

        private void validateOtherResults(LandingReport report)
        {
            var brkList = perfTable.BrakesAvailable(para.SurfaceCondition);

            for (int i = 0; i < brkList.Length; i++)
            {
                if (i == para.BrakeIndex)
                {
                    report.AddOtherResult();
                }
                else
                {
                    double disReq = RoundToInt(calc.DistanceRequiredMeter(i));
                    double disRemain = para.RwyLengthMeter - disReq;

                    if (disRemain >= 0.0)
                    {
                        report.AddOtherResult(brkList[i],
                                              RoundToInt(disReq),
                                              RoundToInt(disRemain));
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }
    }
}
