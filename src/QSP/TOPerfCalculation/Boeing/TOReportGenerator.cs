using QSP.Common;
using QSP.TOPerfCalculation.Boeing.PerfData;
using QSP.MathTools;

namespace QSP.TOPerfCalculation.Boeing
{
    public class TOReportGenerator
    {
        private TOCalculator calc;
        private IndividualPerfTable table;
        private TOParameters para;

        public TOReportGenerator(BoeingPerfTable item, TOParameters para)
        {
            calc = new TOCalculator(item, para);
            table = item.Tables[para.FlapsIndex];
            this.para = para;
        }

        /// <summary>
        /// Computes runway length required for take off, 
        /// for user input and all available assumed temperatures.
        /// </summary>
        /// <exception cref="RunwayTooShortException">
        /// <exception cref="PoorClimbPerformanceException">
        public TOReport TakeOffReport()
        {
            var result = new TOReport();

            var fieldLimitWtTable = para.SurfaceWet ?
                table.WeightTableWet :
                table.WeightTableDry;

            int maxOat = Numbers.RoundToInt(fieldLimitWtTable.MaxOat);
            const int tempIncrement = 1;

            int mainOat = Numbers.RoundToInt(para.OatCelsius);
            double rwyRequired = calc.TakeoffDistanceMeter(mainOat);
            ValidateMainResult(result, mainOat, rwyRequired);

            for (int oat = mainOat + 1; oat <= maxOat; oat += tempIncrement)
            {
                rwyRequired = calc.TakeoffDistanceMeter(oat);
                if (!TryAddResult(result, oat, rwyRequired)) return result;
            }

            return result;
        }

        private void ValidateMainResult(TOReport result, int oat, double rwyRequired)
        {
            if (rwyRequired <= para.RwyLengthMeter)
            {
                if (calc.ClimbLimitWeightTon(oat) * 1000.0 >= para.WeightKg)
                {
                    result.SetPrimaryResult(
                        Numbers.RoundToInt(para.OatCelsius),
                        Numbers.RoundToInt(rwyRequired),
                        Numbers.RoundToInt(para.RwyLengthMeter - rwyRequired));
                }
                else
                {
                    throw new PoorClimbPerformanceException();
                }
            }
            else
            {
                throw new RunwayTooShortException();
            }
        }

        // returns whether the result was successfully added.
        private bool TryAddResult(TOReport result, int oat, double rwyRequired)
        {
            if (rwyRequired <= para.RwyLengthMeter &&
                calc.ClimbLimitWeightTon(oat) * 1000.0 >= para.WeightKg)
            {
                result.AddAssumedTemp(
                    oat,
                    Numbers.RoundToInt(rwyRequired),
                    Numbers.RoundToInt(para.RwyLengthMeter - rwyRequired));

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
