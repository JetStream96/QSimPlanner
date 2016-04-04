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
        public LandingCalcResult GetLandingReport()
        {
            var result = new LandingCalcResult();
            var brkList = perfTable.BrakesAvailable(para.SurfaceCondition);

            //compute the user input
            int disReqMeter = RoundToInt(GetLandingDistanceMeter());

            int disRemainMeter = para.RwyLengthMeter - disReqMeter;

            if (disRemainMeter >= 0)
            {
                result.SetSelectedBrakesResult(
                    brkList[para.BrakeIndex], disReqMeter, disRemainMeter);
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
                    disReqMeter = RoundToInt(GetLandingDistanceMeter(i));
                    disRemainMeter = para.RwyLengthMeter - disReqMeter;

                    if (disRemainMeter >= 0)
                    {
                        result.AddOtherResult(brkList[i],
                                              disReqMeter,
                                              disRemainMeter);
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
        public double GetLandingDistanceMeter(int brakeSetting)
        {
            double totalDisMeter =
                reqData(DataColumn.RefDis, brakeSetting) +
                wtCorrection(brakeSetting) +
                elevationCorrection(brakeSetting) +
                windCorrection(brakeSetting) +
                slopeCorrection(para, brakeSetting);

            double tempExcess = para.TempCelsius - IsaTemp(para.ElevationFT);

            totalDisMeter += tempExcess / 10 * tempExcess >= 0 ?
                reqData(DataColumn.TempAboveISA, brakeSetting) :
                -reqData(DataColumn.TempBelowISA, brakeSetting);

            totalDisMeter += para.AppSpeedIncrease / 10;

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

        private double slopeCorrection(LandingParameters para, int brake)
        {
            double corrPerDegree =
                para.SlopePercent <= 0 ?
                -reqData(DataColumn.DownhillCorr, brake) :
                reqData(DataColumn.UphillCorr, brake);

            return para.SlopePercent * corrPerDegree;
        }

        /// <summary>
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double GetLandingDistanceMeter()
        {
            return GetLandingDistanceMeter(para.BrakeIndex);
        }
    }
}
