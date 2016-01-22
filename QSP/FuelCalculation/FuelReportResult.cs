using System;
using System.Text;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.TimeFormat;

namespace QSP
{

    public class FuelReportResult
    {
        private const int LEFTPAD = 11;
        private const int RIGHTPAD = 7;

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
            ExtraKG = para.ExtraFuel_KG;
            HoldKg = para.HoldingMin * fuelCalc.holdingFuelPerMinuteKg;
            ApuKg = para.APUTime * fuelCalc.apu_fuel_per_min_kg;
            TaxiKg = para.TaxiTime * fuelCalc.taxi_fuel_per_min_kg;
            FinalRsvKg = para.FinalRsvMin * fuelCalc.holdingFuelPerMinuteKg;
            TimeToDest = fuelCalc.TimeToDest;
            TimeToAltn = fuelCalc.TimeToAltn;
            TimeExtra = (int)(para.ExtraFuel_KG / fuelCalc.holdingFuelPerMinuteKg);
            TimeHold = (int)para.HoldingMin;
            TimeFinalRsv = (int)para.FinalRsvMin;
            TimeApu = (int)para.APUTime;
            TimeTaxi = (int)para.TaxiTime;

            setAdditionalPara();
        }


        private void setAdditionalPara()
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

                    TripFuelDisplay = (int)(FuelToDestTon * 1000 * KG_LB);
                    contingencyDisplay = (int)(ContKg * KG_LB);
                    holdDisplay = (int)(HoldKg * KG_LB);
                    extraDisplay = (int)(ExtraKG * KG_LB);
                    alternateDisplay = (int)(FuelToAltnTon * 1000 * KG_LB);
                    finalRsvDisplay = (int)(FinalRsvKg * KG_LB);
                    takeoff_display = (int)(TakeoffFuelKg * KG_LB);
                    apu_display = (int)(ApuKg * KG_LB);
                    taxi_display = (int)(TaxiKg * KG_LB);
                    TotalFuelDisplay = (int)(TotalFuelKG * KG_LB);
                    break;
            }

            string trip_s = lineFormat("TRIP", TripFuelDisplay);
            string contingency_s = lineFormat("CONTINGENCY", contingencyDisplay);
            string hold_s = lineFormat("HOLD", holdDisplay);
            string extra_s = lineFormat("EXTRA", extraDisplay);
            string alternate_s = lineFormat("ALTERNATE", alternateDisplay);
            string final_rsv_s = lineFormat("FINAL RSV", finalRsvDisplay);
            string takeoff_s = lineFormat("AT T/O", takeoff_display);
            string apu_s = lineFormat("APU", apu_display);
            string taxi_s = lineFormat("TAXI", taxi_display);
            string total_s = lineFormat("TOTAL", TotalFuelDisplay);
            string fmc_rsv_s = lineFormatOneDecimalPlace("FMC RSV", (alternateDisplay + finalRsvDisplay) / 1000);

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

            OutputText.AppendLine(wtUnitDisplay + Environment.NewLine + Environment.NewLine + "              FUEL  TIME");
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

        private string lineFormat(string item, int value)
        {
            return item.PadRight(LEFTPAD, ' ') + value.ToString().PadLeft(RIGHTPAD, ' ');
        }

        private string lineFormatOneDecimalPlace(string item, double value)
        {
            return item.PadRight(LEFTPAD, ' ') + value.ToString("0.0").PadLeft(RIGHTPAD, ' ');
        }

    }
}
