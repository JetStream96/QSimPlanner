using QSP.Core;
using QSP.LandingPerfCalculation.Boeing.PerfData;
using static QSP.MathTools.Doubles;
using static QSP.AviationTools.CoversionTools;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class LandingCalculator
    {
        private BoeingPerfTable perfTable;
        private LandingParameters para;

        public LandingCalculator(BoeingPerfTable perfTable,
                                 LandingParameters para)
        {
            this.perfTable = perfTable;
            this.para = para;
        }

        // Based on landing parameters with brake setting overriden, 
        // gets the requested data.
        private double reqData(DataColumn column, int brakeSetting)
        {
            if (para.SurfaceCondition == SurfaceCondition.Dry)
            {
                return perfTable
                        .DataDry
                        .GetValue(para.FlapsIndex, brakeSetting, column);
            }
            return perfTable
                    .DataWet
                    .GetValue(para.FlapsIndex,
                              para.SurfaceCondition,
                              brakeSetting,
                              column);
        }

        /// <exception cref="RunwayTooShortException"></exception>
        public LandingCalcResult LandingReport()
        {
            var result = new LandingCalcResult();
            var brkList = perfTable.BrakesAvailable(para.SurfaceCondition);

            //compute the user input
            double disReqMeter = DistanceRequiredMeter();

            double disRemainMeter = para.RwyLengthMeter - disReqMeter;

            if (disRemainMeter >= 0)
            {
                result.SetSelectedBrakesResult(
                    brkList[para.BrakeIndex],
                    RoundToInt(disReqMeter),
                    RoundToInt(disRemainMeter));
            }
            else
            {
                throw new RunwayTooShortException();
            }

            //compute all possible brake settings

            for (int i = 0; i < brkList.Length; i++)
            {
                if (i == para.BrakeIndex)
                {
                    result.AddOtherResult();
                }
                else
                {
                    disReqMeter = RoundToInt(DistanceRequiredMeter(i));
                    disRemainMeter = para.RwyLengthMeter - disReqMeter;

                    if (disRemainMeter >= 0)
                    {
                        result.AddOtherResult(brkList[i],
                                              RoundToInt(disReqMeter),
                                              RoundToInt(disRemainMeter));
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
        public double DistanceRequiredMeter(int brakeSetting)
        {
            double totalDisMeter =
                reqData(DataColumn.RefDis, brakeSetting) +
                wtCorrection(brakeSetting) +
                elevationCorrection(brakeSetting) +
                windCorrection(brakeSetting) +
                slopeCorrection(brakeSetting) +
                tempCorrection(brakeSetting) +
                appSpdCorrection(brakeSetting);

            if (para.Reverser == ReverserOption.HalfRev)
            {
                totalDisMeter += reqData(DataColumn.HalfRev, brakeSetting);
            }
            else if (para.Reverser == ReverserOption.NoRev)
            {
                totalDisMeter += reqData(DataColumn.NoRev, brakeSetting);
            }

            return totalDisMeter;
        }

        private double wtCorrection(int brakeSetting)
        {
            double wtExcessSteps =
                (para.WeightKG - perfTable.WeightRef) / perfTable.WeightStep;

            return wtExcessSteps *

                (wtExcessSteps >= 0 ?
                reqData(DataColumn.WtAdjustAbove, brakeSetting) :
                -reqData(DataColumn.WtAdjustBelow, brakeSetting));
        }

        private double elevationCorrection(int brake)
        {
            double corrPer1000ft = reqData(DataColumn.AltAdjust, brake);
            return para.ElevationFT / 1000.0 * corrPer1000ft;
        }

        private double windCorrection(int brake)
        {
            double corrPer10kts =
                para.HeadwindKts >= 0 ?
                reqData(DataColumn.HeadwindCorr, brake) :
                -reqData(DataColumn.TailwindCorr, brake);

            return para.HeadwindKts / 10.0 * corrPer10kts;
        }

        private double slopeCorrection(int brake)
        {
            double corrPerDegree =
                para.SlopePercent <= 0 ?
                -reqData(DataColumn.DownhillCorr, brake) :
                reqData(DataColumn.UphillCorr, brake);

            return para.SlopePercent * corrPerDegree;
        }

        private double tempCorrection(int brake)
        {
            double tempExcess = para.TempCelsius - IsaTemp(para.ElevationFT);
            double corrPer10deg = tempExcess >= 0.0 ?
                   reqData(DataColumn.TempAboveISA, brake) :
                   -reqData(DataColumn.TempBelowISA, brake);

            return tempExcess / 10.0 * corrPer10deg;
        }

        private double appSpdCorrection(int brake)
        {
            return para.AppSpeedIncrease / 10.0 *
                reqData(DataColumn.AppSpdAdjust, brake);
        }

        /// <summary>
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double DistanceRequiredMeter()
        {
            return DistanceRequiredMeter(para.BrakeIndex);
        }
    }
}
