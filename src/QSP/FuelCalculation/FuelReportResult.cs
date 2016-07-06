using QSP.Utilities.Units;
using System;
using System.Text;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.TimeFormat;

namespace QSP
{
    public class FuelReportResult
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
        public int TimeToDest { get; private set; }
        public int TimeToAltn { get; private set; }
        public int TimeExtra { get; private set; }
        public int TimeHold { get; private set; }
        public int TimeFinalRsv { get; private set; }
        public int TimeApu { get; private set; }
        public int TimeTaxi { get; private set; }

        // Additional Results
        public double TakeoffFuelKg { get; private set; }
        public double LdgFuelKgPredict { get; private set; }
        public double TotalFuelKG { get; private set; }

        public FuelReportResult(
             double FuelToDestTon,
             double FuelToAltnTon,
             double ContKg,
             double ExtraKG,
             double HoldKg,
             double ApuKg,
             double TaxiKg,
             double FinalRsvKg,
             int TimeToDest,
             int TimeToAltn,
             int TimeExtra,
             int TimeHold,
             int TimeFinalRsv,
             int TimeApu,
             int TimeTaxi)
        {
            this.FuelToDestTon = FuelToDestTon;
            this.FuelToAltnTon = FuelToAltnTon;
            this.ContKg = ContKg;
            this.ExtraKG = ExtraKG;
            this.HoldKg = HoldKg;
            this.ApuKg = ApuKg;
            this.TaxiKg = TaxiKg;
            this.FinalRsvKg = FinalRsvKg;
            this.TimeToDest = TimeToDest;
            this.TimeToAltn = TimeToAltn;
            this.TimeExtra = TimeExtra;
            this.TimeHold = TimeHold;
            this.TimeFinalRsv = TimeFinalRsv;
            this.TimeApu = TimeApu;
            this.TimeTaxi = TimeTaxi;

            SetAdditionalPara();
        }
        
        private void SetAdditionalPara()
        {
            TakeoffFuelKg = FuelToDestTon * 1000 + ContKg + HoldKg +
                ExtraKG + FuelToAltnTon * 1000 + FinalRsvKg;

            LdgFuelKgPredict = TakeoffFuelKg - FuelToDestTon * 1000;

            TotalFuelKG = FuelToDestTon * 1000 + ContKg + HoldKg +
                ExtraKG + ApuKg + TaxiKg + FuelToAltnTon * 1000 + FinalRsvKg;
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
            string fmc_rsv_s = LineFormatOneDecimalPlace("FMC RSV", (alternateDisplay + finalRsvDisplay) / 1000);

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

            int time_cont = TimeToDest / 20;
            int time_TO = TimeToDest + time_cont + TimeExtra + TimeHold + TimeFinalRsv + TimeToAltn;
            int time_total = time_TO + TimeApu + TimeTaxi;

            StringBuilder OutputText = new StringBuilder();

            OutputText.AppendLine(wtUnitDisplay + "\n\n              FUEL  TIME");
            OutputText.AppendLine(trip_s + "  " + MinToHHMM(TimeToDest));
            OutputText.AppendLine(contingency_s + "  " + MinToHHMM(time_cont));
            OutputText.AppendLine(hold_s + "  " + MinToHHMM(TimeHold));
            OutputText.AppendLine(extra_s + "  " + MinToHHMM(TimeExtra));
            OutputText.AppendLine(alternate_s + "  " + MinToHHMM(TimeToAltn));
            OutputText.AppendLine(final_rsv_s + "  " + MinToHHMM(TimeFinalRsv) + Environment.NewLine);
            OutputText.AppendLine(takeoff_s + "  " + MinToHHMM(time_TO) + Environment.NewLine);
            OutputText.AppendLine(apu_s + "  " + MinToHHMM(TimeApu));
            OutputText.AppendLine(taxi_s + "  " + MinToHHMM(TimeTaxi) + Environment.NewLine);
            OutputText.AppendLine(total_s + "  " + MinToHHMM(time_total));
            OutputText.Append(fmc_rsv_s);

            return OutputText.ToString();
        }

        private string LineFormat(string item, int value)
        {
            return item.PadRight(LeftPad, ' ') + value.ToString().PadLeft(RightPad, ' ');
        }

        private string LineFormatOneDecimalPlace(string item, double value)
        {
            return item.PadRight(LeftPad, ' ') + value.ToString("0.0").PadLeft(RightPad, ' ');
        }

    }
}
