using QSP.Core;
using QSP.LandingPerfCalculation.Boeing.PerfData;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class Calculator
    {
        private BoeingPerfTable perfTable;

        public Calculator(BoeingPerfTable perfTable)
        {
            this.perfTable = perfTable;
        }

        // Based on landing parameters with brake setting overriden, gets the requested data.
        private double reqData(LandingParameters para, DataColumn column, int brakeSetting)
        {
            if (para.SurfaceCondition == SurfaceCondition.Dry)
            {
                return perfTable
                        .DataDry
                        .GetValue(para.FlapsIndex, brakeSetting, column);
            }
            return perfTable
                    .DataWet
                    .GetValue(para.FlapsIndex, para.SurfaceCondition, brakeSetting, column);
        }

        public LandingCalcResult GetLandingReport(LandingParameters para)
        {
            LandingCalcResult result = new LandingCalcResult();

            var brkList = perfTable.BrakesAvailable(para.SurfaceCondition);

            //compute the user input
            int disReqMeter = (int)(GetLandingDistanceMeter(para, para.AutoBrakeIndex));
            int disRemainMeter = para.RwyLengthMeter - disReqMeter;

            if (disRemainMeter >= 0)
            {
                result.SetSelectedBrakesResult(
                    brkList[para.AutoBrakeIndex], disReqMeter, disRemainMeter);
            }
            else
            {
                throw new RunwayTooShortException();
            }

            //compute all possible brake settings

            for (int i = 0; i < brkList.Length; i++)
            {
                if (i == para.AutoBrakeIndex)
                {
                    result.AddOtherResult();
                }
                else
                {
                    disReqMeter = (int)(GetLandingDistanceMeter(para, i));
                    disRemainMeter = para.RwyLengthMeter - disReqMeter;

                    if (disRemainMeter >= 0)
                    {
                        result.AddOtherResult(brkList[i], disReqMeter, disRemainMeter);
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double GetLandingDistanceMeter(LandingParameters para, int brakeSetting)
        {
            double wtExcessSteps = (para.WeightKG - perfTable.weightRef) / perfTable.weightStep;

            double totalDisMeter = reqData(para, DataColumn.RefDis, brakeSetting) +
                wtExcessSteps *

                (wtExcessSteps >= 0 ?
                reqData(para, DataColumn.WtAdjustAbove, brakeSetting) :
                -reqData(para, DataColumn.WtAdjustBelow, brakeSetting))

                + para.ElevationFT / 1000 * reqData(para, DataColumn.AltAdjust, brakeSetting)
                + para.HeadwindKts / 10 *

                (para.HeadwindKts >= 0 ?
                reqData(para, DataColumn.HeadwindCorr, brakeSetting) :
                -reqData(para, DataColumn.TailwindCorr, brakeSetting))

                + para.SlopePercent *

                (para.SlopePercent <= 0 ?
                -reqData(para, DataColumn.DownhillCorr, brakeSetting) :
                reqData(para, DataColumn.UphillCorr, brakeSetting));

            double tempExcess = para.TempCelsius - AviationTools.CoversionTools.IsaTemp(para.ElevationFT);

            totalDisMeter += tempExcess / 10 * tempExcess >= 0 ?
                reqData(para, DataColumn.TempAboveISA, brakeSetting) :
                -reqData(para, DataColumn.TempBelowISA, brakeSetting);

            totalDisMeter += para.AppSpeedIncrease / 10;

            if (para.Reverser == ReverserOption.HalfRev)
            {
                totalDisMeter += reqData(para, DataColumn.HalfRev, brakeSetting);
            }
            else if (para.Reverser == ReverserOption.NoRev)
            {
                totalDisMeter += reqData(para, DataColumn.NoRev, brakeSetting);
            }

            return totalDisMeter;
        }

        /// <summary>
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double GetLandingDistanceMeter(LandingParameters para)
        {
            return GetLandingDistanceMeter(para, para.AutoBrakeIndex);
        }
    }
}
