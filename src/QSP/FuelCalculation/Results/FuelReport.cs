using QSP.Utilities.Units;
using System.Linq;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.TimeFormat;
using static QSP.MathTools.Doubles;
using static QSP.Utilities.Units.Conversions;

namespace QSP.FuelCalculation.Results
{
    // Unit of fuel is in kg, time in minutes.

    public class FuelReport
    {
        private const int RightPad = 11;
        private const int LeftPad = 7;

        // Passed in via constructor
        public double FuelToDest { get; }
        public double FuelToAltn { get; }
        public double FuelCont { get; }
        public double FuelExtra { get; }
        public double FuelHold { get; }
        public double FuelApu { get; }
        public double FuelTaxi { get; }
        public double FuelFinalRsv { get; }
        public double TimeToDest { get; }
        public double TimeToAltn { get; }
        public double TimeCont { get; }
        public double TimeExtra { get; }
        public double TimeHold { get; }
        public double TimeFinalRsv { get; }
        public double TimeApu { get; }
        public double TimeTaxi { get; }

        // Additional Results
        public double TakeoffFuel { get; private set; }
        public double PredictedLdgFuel { get; private set; }
        public double TotalFuel { get; private set; }
        public double FuelFmcRsv { get; private set; }
        public double TimeTakeoff { get; private set; }
        public double TimeTotal { get; private set; }

        public FuelReport(
             double FuelToDest,
             double FuelToAltn,
             double FuelCont,
             double FuelExtra,
             double FuelHold,
             double FuelApu,
             double FuelTaxi,
             double FuelFinalRsv,
             double TimeToDest,
             double TimeToAltn,
             double TimeCont,
             double TimeExtra,
             double TimeHold,
             double TimeFinalRsv,
             double TimeApu,
             double TimeTaxi)
        {
            this.FuelToDest = FuelToDest;
            this.FuelToAltn = FuelToAltn;
            this.FuelCont = FuelCont;
            this.FuelExtra = FuelExtra;
            this.FuelHold = FuelHold;
            this.FuelApu = FuelApu;
            this.FuelTaxi = FuelTaxi;
            this.FuelFinalRsv = FuelFinalRsv;
            this.TimeToDest = TimeToDest;
            this.TimeToAltn = TimeToAltn;
            this.TimeCont = TimeCont;
            this.TimeExtra = TimeExtra;
            this.TimeHold = TimeHold;
            this.TimeFinalRsv = TimeFinalRsv;
            this.TimeApu = TimeApu;
            this.TimeTaxi = TimeTaxi;

            SetAdditionalPara();
        }

        private void SetAdditionalPara()
        {
            PredictedLdgFuel = FuelCont + FuelHold + FuelExtra + FuelToAltn +
                FuelFinalRsv;
            TakeoffFuel = PredictedLdgFuel + FuelToDest;
            TotalFuel = TakeoffFuel + FuelApu + FuelTaxi;
            FuelFmcRsv = FuelToAltn + FuelFinalRsv;
            TimeTakeoff = TimeToDest + TimeCont + TimeExtra + TimeHold +
                TimeFinalRsv + TimeToAltn;
            TimeTotal = TimeTakeoff + TimeApu + TimeTaxi;
        }

        public string ToString(WeightUnit unit)
        {
            var valuesKg = new[]
            {
                FuelToDest, FuelCont, FuelHold, FuelExtra, FuelToAltn,
                FuelFinalRsv, TakeoffFuel, FuelApu, FuelTaxi, TotalFuel
            };

            var names = new[]
            {
                "TRIP", "CONTINGENCY", "HOLD", "EXTRA", "ALTERNATE",
                "FINAL RSV", "AT T/O", "APU", "TAXI", "TOTAL"
            };

            var times = new[]
            {
                TimeToDest, TimeCont, TimeHold, TimeExtra, TimeToAltn,
                TimeFinalRsv, TimeTakeoff, TimeApu, TimeTaxi, TimeTotal
            };

            var linebreakCount = new[]
            {
                1, 1, 1, 1, 1, 2, 1, 2, 1, 1, 2, 1
            };

            var ratio = unit == WeightUnit.KG ? 1.0 : KgLbRatio;
            var value = valuesKg.Select(x => (x * ratio).ToString("F0"));
            var timeStr = times.Select(t => MinToHHMM(RoundToInt(t)));
            var lines = names
                .Zip(value, (n, v) => LineFormat(n, v))
                .Zip(timeStr, (s, t) => s + "  " + t);
            var linebreaks = linebreakCount.Select(c => new string('\n', c));
            var combined = lines.Zip(linebreaks, (s, b) => s + b);
            var joined = string.Concat(combined);

            var wt = "ALL WEIGHTS IN " + WeightUnitToString(unit);
            return wt + "\n\n" + "FUEL  TIME".PadLeft(RightPad + LeftPad) +
                "\n" + joined +
                LineFormat("FMC RSV", FuelFmcRsv.ToString("F1"));
        }

        private static string MinToString(double minutes)
        {
            return MinToHHMM(RoundToInt(minutes));
        }

        private static string LineFormat(string s1, string s2)
        {
            return s1.PadRight(RightPad) + s2.PadLeft(LeftPad);
        }
    }
}
