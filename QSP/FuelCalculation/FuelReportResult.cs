using System;
using System.Text;
using QSP.TimeFormatTools;
using QSP.AviationTools;
using static QSP.AviationTools.AviationConstants;
using static QSP.TimeFormatTools.TimeFormat;

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
            int contingency_display = 0;
            int hold_display = 0;
            int extra_display = 0;
            int alternate_display = 0;
            int takeoff_display = 0;
            int apu_display = 0;
            int taxi_display = 0;
            int final_rsv_display = 0;

            switch (unit)
            {
                case WeightUnit.KG:

                    TripFuelDisplay = (int)(FuelToDestTon * 1000);
                    contingency_display = (int)ContKg;
                    hold_display = (int)HoldKg;
                    extra_display = (int)ExtraKG;
                    alternate_display = (int)(FuelToAltnTon * 1000);
                    final_rsv_display = (int)FinalRsvKg;
                    takeoff_display = (int)TakeoffFuelKg;
                    apu_display = (int)ApuKg;
                    taxi_display = (int)TaxiKg;
                    TotalFuelDisplay = (int)TotalFuelKG;
                    break;

                case WeightUnit.LB:

                    TripFuelDisplay = (int)(FuelToDestTon * 1000 * KG_LB);
                    contingency_display = (int)(ContKg * KG_LB);
                    hold_display = (int)(HoldKg * KG_LB);
                    extra_display = (int)(ExtraKG * KG_LB);
                    alternate_display = (int)(FuelToAltnTon * 1000 * KG_LB);
                    final_rsv_display = (int)(FinalRsvKg * KG_LB);
                    takeoff_display = (int)(TakeoffFuelKg * KG_LB);
                    apu_display = (int)(ApuKg * KG_LB);
                    taxi_display = (int)(TaxiKg * KG_LB);
                    TotalFuelDisplay = (int)(TotalFuelKG * KG_LB);

                    break;
            }

            string trip_s = lineFormat("TRIP", TripFuelDisplay);
            string contingency_s = lineFormat("CONTINGENCY", contingency_display);
            string hold_s = lineFormat("HOLD", hold_display);
            string extra_s = lineFormat("EXTRA", extra_display);
            string alternate_s = lineFormat("ALTERNATE", alternate_display);
            string final_rsv_s = lineFormat("FINAL RSV", final_rsv_display);
            string takeoff_s = lineFormat("AT T/O", takeoff_display);
            string apu_s = lineFormat("APU", apu_display);
            string taxi_s = lineFormat("TAXI", taxi_display);
            string total_s = lineFormat("TOTAL", TotalFuelDisplay);

            //This part man ...
            //=======================================================
            string fmc_rsv_s = null;
            int i = 0;
            fmc_rsv_s = "FMC RSV";
            while (Math.Truncate(Math.Log10(alternate_display + final_rsv_display)) + i < LEFTPAD )
            {
                fmc_rsv_s += " ";
                i++;
            }

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

            int fmc_reserve1 = 0;
            int fmc_reserve2 = 0;
            fmc_reserve1 = (alternate_display + final_rsv_display) / 1000;
            fmc_reserve2 = ((alternate_display + final_rsv_display) - fmc_reserve1 * 1000) / 100 + 1;
            if (fmc_reserve2 == 10)
            {
                fmc_reserve1++;
                fmc_reserve2 = 0;
            }
            //======================================================

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
            OutputText.Append(fmc_rsv_s + fmc_reserve1 + "." + fmc_reserve2);

            return OutputText.ToString();
        }

        private string lineFormat(string item, int value)
        {
            return item.PadRight(LEFTPAD, ' ') + value.ToString().PadLeft(RIGHTPAD, ' ');
        }

    }
}
