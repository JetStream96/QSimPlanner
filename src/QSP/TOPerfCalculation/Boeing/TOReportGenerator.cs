using System.Collections.Generic;
using System.Linq;
using QSP.Common;
using QSP.TOPerfCalculation.Boeing.PerfData;
using QSP.Utilities;

namespace QSP.TOPerfCalculation.Boeing
{
    public class TOReportGenerator
    {
        private readonly TOCalculator calc;
        private readonly IndividualPerfTable table;
        private readonly TOParameters para;

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
        /// <param name="tempIncrement">Temperature increment in Celsius.</param>
        /// <exception cref="RunwayTooShortException"></exception>
        /// <exception cref="PoorClimbPerformanceException"></exception>
        public TOReport TakeOffReport(double tempIncrement = 1.0)
        {
            double mainOat = para.OatCelsius;
            double rwyRequired = calc.TakeoffDistanceMeter(mainOat);
            var mainResult = ValidateResult(mainOat, rwyRequired);
            var assumedTemp = AssumedTempResults(tempIncrement);

            return new TOReport(mainResult, assumedTemp.ToList());
        }

        private IEnumerable<TOReportRow> AssumedTempResults(double tempIncrement)
        {
            var fieldLimitWtTable = para.SurfaceWet ?
                table.WeightTableWet :
                table.WeightTableDry;

            double maxOat = fieldLimitWtTable.MaxOat;

            for (double oat = para.OatCelsius + tempIncrement; oat <= maxOat; oat += tempIncrement)
            {
                var rwyRequired = calc.TakeoffDistanceMeter(oat);
                var row = TryValidateResult(oat, rwyRequired);
                if (row == null) break;
                yield return row;
            }

            yield break;
        }

        private TOReportRow ValidateResult(double oat, double rwyRequired)
        {
            if (rwyRequired > para.RwyLengthMeter) throw new RunwayTooShortException();
            var climbLim = calc.ClimbLimitWeightTon(oat) * 1000.0;
            if (climbLim < para.WeightKg) throw new PoorClimbPerformanceException();
            return new TOReportRow(oat, rwyRequired, para.RwyLengthMeter - rwyRequired);
        }

        // Returns null if not valid.
        private TOReportRow TryValidateResult(double oat, double rwyRequired)
        {
            return ExceptionHelpers.DefaultIfThrows(() => ValidateResult(oat, rwyRequired));
        }
    }
}
