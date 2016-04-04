using QSP.Core;
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
            table = item.GetTable(para.FlapsIndex);
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

            int maxOat = Doubles.RoundToInt(fieldLimitWtTable.MaxOat);
            const int tempIncrement = 1;

            for (int oat = Doubles.RoundToInt(para.OatCelsius);
                 oat <= maxOat;
                 oat += tempIncrement)
            {
                double rwyRequired = calc.TakeoffDistanceMeter(oat);

                if (oat == para.OatCelsius)
                {
                    validateMainResult(result, oat, rwyRequired);
                }
                else if (tryAddResult(result, oat, rwyRequired) == false)
                {
                    return result;
                }
            }

            return result;
        }

        private void validateMainResult(TOReport result, 
                                        int oat,
                                        double rwyRequired)
        {
            if (rwyRequired <= para.RwyLengthMeter)
            {
                if (calc.ClimbLimitWeightTon(oat) * 1000.0 >= para.WeightKg)
                {
                    result.SetPrimaryResult(
                        Doubles.RoundToInt(para.OatCelsius),
                        Doubles.RoundToInt(rwyRequired),
                        Doubles.RoundToInt(para.RwyLengthMeter - rwyRequired));
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
        private bool tryAddResult(TOReport result, int oat,
            double rwyRequired)
        {
            if (rwyRequired <= para.RwyLengthMeter &&
                calc.ClimbLimitWeightTon(oat) * 1000.0 >= para.WeightKg)
            {
                result.AddAssumedTemp(
                    oat,
                    Doubles.RoundToInt(rwyRequired),
                    Doubles.RoundToInt(para.RwyLengthMeter - rwyRequired));

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
