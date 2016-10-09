using QSP.Utilities.Units;
using System;
using System.Text;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.TimeFormat;
using static QSP.MathTools.Doubles;

namespace QSP.FuelCalculation
{
    public class FuelReport
    {
        private const int LeftPad = 11;
        private const int RightPad = 7;

        // Passed in via constructor
        public double FuelToDestTon { get; private set; }
        public double FuelToAltnTon { get; private set; }
        public double ContKg { get; private set; }
        public double ExtraKG { get; private set; }
        public double HoldKg { get; private set; }
        public double ApuKg { get; private set; }
        public double TaxiKg { get; private set; }
        public double FinalRsvKg { get; private set; }
        public double TimeToDestMin { get; private set; }
        public double TimeToAltnMin { get; private set; }
        public double TimeExtraMin { get; private set; }
        public double TimeHoldMin { get; private set; }
        public double TimeFinalRsvMin { get; private set; }
        public double TimeApuMin { get; private set; }
        public double TimeTaxiMin { get; private set; }

        // Additional Results
        public double TakeoffFuelKg { get; private set; }
        public double LdgFuelKgPredict { get; private set; }
        public double TotalFuelKG { get; private set; }

        public FuelReport(
             double FuelToDestTon,
             double FuelToAltnTon,
             double ContKg,
             double ExtraKG,
             double HoldKg,
             double ApuKg,
             double TaxiKg,
             double FinalRsvKg,
             double TimeToDestMin,
             double TimeToAltnMin,
             double TimeExtraMin,
             double TimeHoldMin,
             double TimeFinalRsvMin,
             double TimeApuMin,
             double TimeTaxiMin)
        {
            this.FuelToDestTon = FuelToDestTon;
            this.FuelToAltnTon = FuelToAltnTon;
            this.ContKg = ContKg;
            this.ExtraKG = ExtraKG;
            this.HoldKg = HoldKg;
            this.ApuKg = ApuKg;
            this.TaxiKg = TaxiKg;
            this.FinalRsvKg = FinalRsvKg;
            this.TimeToDestMin = TimeToDestMin;
            this.TimeToAltnMin = TimeToAltnMin;
            this.TimeExtraMin = TimeExtraMin;
            this.TimeHoldMin = TimeHoldMin;
            this.TimeFinalRsvMin = TimeFinalRsvMin;
            this.TimeApuMin = TimeApuMin;
            this.TimeTaxiMin = TimeTaxiMin;

            SetAdditionalPara();
        }
        
        private void SetAdditionalPara()
        {
            TakeoffFuelKg = FuelToDestTon * 1000.0 + ContKg + HoldKg +
                ExtraKG + FuelToAltnTon * 1000.0 + FinalRsvKg;

            LdgFuelKgPredict = TakeoffFuelKg - FuelToDestTon * 1000.0;

            TotalFuelKG = FuelToDestTon * 1000.0 + ContKg + HoldKg +
                ExtraKG + ApuKg + TaxiKg + FuelToAltnTon * 1000.0 + FinalRsvKg;
        }

        public string ToString(WeightUnit unit)
        {
            int TripFuelDisplay = 0;
            int TotalFuelDisplay = 0;
            int contingencyDisplay = 0;
            int holdDisplay = 0;
            int extraDisplay = 0;
            int alternateDisplay = 0;
            int takeoff_display = 0;
            int apu_display = 0;
            int taxi_display = 0;
            int finalRsvDisplay = 0;

            switch (unit)
            {
                case WeightUnit.KG:

                    TripFuelDisplay = (int)(FuelToDestTon * 1000);
                    contingencyDisplay = (int)ContKg;
                    holdDisplay = (int)HoldKg;
                    extraDisplay = (int)ExtraKG;
                    alternateDisplay = (int)(FuelToAltnTon * 1000);
                    finalRsvDisplay = (int)FinalRsvKg;
                    takeoff_display = (int)TakeoffFuelKg;
                    apu_display = (int)ApuKg;
                    taxi_display = (int)TaxiKg;
                    TotalFuelDisplay = (int)TotalFuelKG;
                    break;

                case WeightUnit.LB:

                    TripFuelDisplay = (int)(FuelToDestTon * 1000 * KgLbRatio);
                    contingencyDisplay = (int)(ContKg * KgLbRatio);
                    holdDisplay = (int)(HoldKg * KgLbRatio);
                    extraDisplay = (int)(ExtraKG * KgLbRatio);
                    alternateDisplay = (int)(FuelToAltnTon * 1000 * KgLbRatio);
                    finalRsvDisplay = (int)(FinalRsvKg * KgLbRatio);
                    takeoff_display = (int)(TakeoffFuelKg * KgLbRatio);
                    apu_display = (int)(ApuKg * KgLbRatio);
                    taxi_display = (int)(TaxiKg * KgLbRatio);
                    TotalFuelDisplay = (int)(TotalFuelKG * KgLbRatio);
                    break;
            }

            string trip_s = LineFormat("TRIP", TripFuelDisplay);
            string contingency_s = LineFormat("CONTINGENCY", contingencyDisplay);
            string hold_s = LineFormat("HOLD", holdDisplay);
            string extra_s = LineFormat("EXTRA", extraDisplay);
            string alternate_s = LineFormat("ALTERNATE", alternateDisplay);
            string final_rsv_s = LineFormat("FINAL RSV", finalRsvDisplay);
            string takeoff_s = LineFormat("AT T/O", takeoff_display);
            string apu_s = LineFormat("APU", apu_display);
            string taxi_s = LineFormat("TAXI", taxi_display);
            string total_s = LineFormat("TOTAL", TotalFuelDisplay);
            string fmc_rsv_s = LineFormatOneDecimalPlace("FMC RSV", (alternateDisplay + finalRsvDisplay) / 1000.0);

            string wtUnitDisplay = null;

            switch (unit)
            {
                case WeightUnit.KG:
                    wtUnitDisplay = "ALL WEIGHTS IN KG";
                    break;
                case WeightUnit.LB:
                    wtUnitDisplay = "ALL WEIGHTS IN LB";
                    break;
            }

            double time_cont = TimeToDestMin / 20;
            double time_TO = TimeToDestMin + time_cont + TimeExtraMin + TimeHoldMin + TimeFinalRsvMin + TimeToAltnMin;
            double time_total = time_TO + TimeApuMin + TimeTaxiMin;

            StringBuilder OutputText = new StringBuilder();

            OutputText.AppendLine(wtUnitDisplay + "\n\n              FUEL  TIME");
            OutputText.AppendLine(trip_s + "  " + MinToString(TimeToDestMin));
            OutputText.AppendLine(contingency_s + "  " + MinToString(time_cont));
            OutputText.AppendLine(hold_s + "  " + MinToString(TimeHoldMin));
            OutputText.AppendLine(extra_s + "  " + MinToString(TimeExtraMin));
            OutputText.AppendLine(alternate_s + "  " + MinToString(TimeToAltnMin));
            OutputText.AppendLine(final_rsv_s + "  " + MinToString(TimeFinalRsvMin) + Environment.NewLine);
            OutputText.AppendLine(takeoff_s + "  " + MinToString(time_TO) + Environment.NewLine);
            OutputText.AppendLine(apu_s + "  " + MinToString(TimeApuMin));
            OutputText.AppendLine(taxi_s + "  " + MinToString(TimeTaxiMin) + Environment.NewLine);
            OutputText.AppendLine(total_s + "  " + MinToString(time_total));
            OutputText.Append(fmc_rsv_s);

            return OutputText.ToString();
        }

        private static string MinToString(double minutes)
        {
            return MinToHHMM(RoundToInt(minutes));
        }

        private string LineFormat(string item, int value)
        {
            return item.PadRight(LeftPad, ' ') + 
                value.ToString().PadLeft(RightPad, ' ');
        }

        private string LineFormatOneDecimalPlace(string item, double value)
        {
            return item.PadRight(LeftPad, ' ') + 
                value.ToString("0.0").PadLeft(RightPad, ' ');
        }

    }
}
