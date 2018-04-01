using QSP.LandingPerfCalculation.Boeing.PerfData;
using static QSP.AviationTools.ConversionTools;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class LandingCalculator
    {
        private BoeingPerfTable perfTable;
        private LandingParameters para;

        public LandingCalculator(BoeingPerfTable perfTable, LandingParameters para)
        {
            this.perfTable = perfTable;
            this.para = para;
        }

        // Based on landing parameters with brake setting overriden, 
        // gets the requested data.
        private double ReqData(DataColumn column, int brakeSetting)
        {
            if (para.SurfaceCondition == SurfaceCondition.Dry)
            {
                return perfTable.DataDry.GetValue(para.FlapsIndex, brakeSetting, column);
            }

            return perfTable.DataWet.GetValue(
                para.FlapsIndex, para.SurfaceCondition, brakeSetting, column);
        }

        /// <summary>
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double DistanceRequiredMeter(int brakeSetting)
        {
            double totalDisMeter =
                ReqData(DataColumn.RefDis, brakeSetting) +
                WtCorrection(brakeSetting) +
                ElevationCorrection(brakeSetting) +
                WindCorrection(brakeSetting) +
                SlopeCorrection(brakeSetting) +
                TempCorrection(brakeSetting) +
                AppSpdCorrection(brakeSetting);

            if (para.Reverser == ReverserOption.HalfRev)
            {
                totalDisMeter += ReqData(DataColumn.HalfRev, brakeSetting);
            }

            if (para.Reverser == ReverserOption.NoRev)
            {
                totalDisMeter += ReqData(DataColumn.NoRev, brakeSetting);
            }

            return totalDisMeter;
        }

        private double WtCorrection(int brakeSetting)
        {
            double wtExcessSteps =
                (para.WeightKG - perfTable.WeightRef) / perfTable.WeightStep;

            return wtExcessSteps *
                (wtExcessSteps >= 0 ?
                ReqData(DataColumn.WtAdjustAbove, brakeSetting) :
                -ReqData(DataColumn.WtAdjustBelow, brakeSetting));
        }

        private double ElevationCorrection(int brake)
        {
            double corrPer1000ft = ReqData(DataColumn.AltAdjust, brake);
            double pressAlt = PressureAltitudeFt(para.ElevationFT, para.QNH);
            return pressAlt / 1000.0 * corrPer1000ft;
        }

        private double WindCorrection(int brake)
        {
            double corrPer10kts = para.HeadwindKts >= 0 ?
                ReqData(DataColumn.HeadwindCorr, brake) :
                -ReqData(DataColumn.TailwindCorr, brake);

            return para.HeadwindKts / 10.0 * corrPer10kts;
        }

        private double SlopeCorrection(int brake)
        {
            double corrPerDegree = para.SlopePercent <= 0 ?
                -ReqData(DataColumn.DownhillCorr, brake) :
                ReqData(DataColumn.UphillCorr, brake);

            return para.SlopePercent * corrPerDegree;
        }

        private double TempCorrection(int brake)
        {
            double tempExcess = para.TempCelsius - IsaTemp(para.ElevationFT);
            double corrPer10deg = tempExcess >= 0.0 ?
                   ReqData(DataColumn.TempAboveISA, brake) :
                   -ReqData(DataColumn.TempBelowISA, brake);

            return tempExcess / 10.0 * corrPer10deg;
        }

        private double AppSpdCorrection(int brake)
        {
            return para.AppSpeedIncrease / 10.0 * ReqData(DataColumn.AppSpdAdjust, brake);
        }

        /// <summary>
        /// Gets the landing distance for the given landing parameters.
        /// </summary>
        public double DistanceRequiredMeter() => DistanceRequiredMeter(para.BrakeIndex);
    }
}
