using System;
using System.Text;
using QSP.FuelCalculation;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.TimeFormat;
using QSP.Utilities.Units;

namespace QSP
{

    public class FuelReportResult
    {
        private const int LeftPad = 11;
        private const int RightPad = 7;

        public double FuelToDestTon;
        public double FuelToAltnTon;
        public double ContKg;
        public double ExtraKG;
        public double HoldKg;
        public double ApuKg;
        public double TaxiKg;
        public double FinalRsvKg;
        public int TimeToDest;
        public int TimeToAltn;
        public int TimeExtra;
        public int TimeHold;
        public int TimeFinalRsv;
        public int TimeApu;
        public int TimeTaxi;
        //' Additional Results
        public double TakeoffFuelKg;
        public double LdgFuelKgPredict;
        public double TotalFuelKG;

        public FuelReportResult(double fuelDestTon, double fuelAltnTon, FuelCalculationParameters para, FuelCalculator fuelCalc)
        {
            FuelToDestTon = fuelDestTon;
            FuelToAltnTon = fuelAltnTon;
            ContKg = FuelToDestTon * 1000 * para.ContPerc / 100;
            ExtraKG = para.ExtraFuelKg;
            HoldKg = para.HoldingMin * fuelCalc.holdingFuelPerMinuteKg;
            ApuKg = para.APUTime * fuelCalc.apuFuelPerMinKg;
            TaxiKg = para.TaxiTime * fuelCalc.taxiFuelPerMinKg;
            FinalRsvKg = para.FinalRsvMin * fuelCalc.holdingFuelPerMinuteKg;
            TimeToDest = fuelCalc.TimeToDest;
            TimeToAltn = fuelCalc.TimeToAltn;
            TimeExtra = (int)(para.ExtraFuelKg / fuelCalc.holdingFuelPerMinuteKg);
            TimeHold = (int)para.HoldingMin;
            TimeFinalRsv = (int)para.FinalRsvMin;
            TimeApu = (int)para.APUTime;
            TimeTaxi = (int)para.TaxiTime;

            SetAdditionalPara();
        }


        private void SetAdditionalPara()
        {
            TakeoffFuelKg = FuelToDestTon * 1000 + ContKg + HoldKg + ExtraKG + FuelToAltnTon * 1000 + FinalRsvKg;
            LdgFuelKgPredict = TakeoffFuelKg - FuelToDestTon * 1000;
            TotalFuelKG = FuelToDestTon * 1000 + ContKg + HoldKg + ExtraKG + ApuKg + TaxiKg + FuelToAltnTon * 1000 + FinalRsvKg;
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
