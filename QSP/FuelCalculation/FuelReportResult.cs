using System;
using System.Text;
using QSP.TimeFormatTools;
using QSP.AviationTools;
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

        public FuelReportResult(double fuel_dest_ton, double fuel_altn_ton, FuelCalculationParameters para, FuelCalculator fuelCalc)
        {
            FuelToDestTon = fuel_dest_ton;
            FuelToAltnTon = fuel_altn_ton;
            ContKg = FuelToDestTon * 1000 * para.ContPerc / 100;
            ExtraKG = para.ExtraFuel_KG;
            HoldKg = para.HoldingMin * fuelCalc.holding_fuel_per_minute_kg;
            ApuKg = para.APUTime * fuelCalc.apu_fuel_per_min_kg;
            TaxiKg = para.TaxiTime * fuelCalc.taxi_fuel_per_min_kg;
            FinalRsvKg = para.FinalRsvMin * fuelCalc.holding_fuel_per_minute_kg;
            TimeToDest = fuelCalc.TimeToDest;
            TimeToAltn = fuelCalc.TimeToAltn;
            TimeExtra = Convert.ToInt32(para.ExtraFuel_KG / fuelCalc.holding_fuel_per_minute_kg);
            TimeHold = Convert.ToInt32(para.HoldingMin);
            TimeFinalRsv = Convert.ToInt32(para.FinalRsvMin);
            TimeApu = Convert.ToInt32(para.APUTime);
            TimeTaxi = Convert.ToInt32(para.TaxiTime);

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

                    TripFuelDisplay = Convert.ToInt32(FuelToDestTon * 1000);
                    contingency_display = Convert.ToInt32(ContKg);
                    hold_display = Convert.ToInt32(HoldKg);
                    extra_display = Convert.ToInt32(ExtraKG);
                    alternate_display = Convert.ToInt32(FuelToAltnTon * 1000);
                    final_rsv_display = Convert.ToInt32(FinalRsvKg);
                    takeoff_display = Convert.ToInt32(TakeoffFuelKg);
                    apu_display = Convert.ToInt32(ApuKg);
                    taxi_display = Convert.ToInt32(TaxiKg);
                    TotalFuelDisplay = Convert.ToInt32(TotalFuelKG);

                    break;
                case WeightUnit.LB:

                    TripFuelDisplay = Convert.ToInt32(FuelToDestTon * 1000 * AviationConstants.KG_LB);
                    contingency_display = Convert.ToInt32(ContKg * AviationConstants.KG_LB);
                    hold_display = Convert.ToInt32(HoldKg * AviationConstants.KG_LB);
                    extra_display = Convert.ToInt32(ExtraKG * AviationConstants.KG_LB);
                    alternate_display = Convert.ToInt32(FuelToAltnTon * 1000 * AviationConstants.KG_LB);
                    final_rsv_display = Convert.ToInt32(FinalRsvKg * AviationConstants.KG_LB);
                    takeoff_display = Convert.ToInt32(TakeoffFuelKg * AviationConstants.KG_LB);
                    apu_display = Convert.ToInt32(ApuKg * AviationConstants.KG_LB);
                    taxi_display = Convert.ToInt32(TaxiKg * AviationConstants.KG_LB);
                    TotalFuelDisplay = Convert.ToInt32(TotalFuelKG * AviationConstants.KG_LB);

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
            while (Math.Truncate(Math.Log10(alternate_display + final_rsv_display)) + i < 11)
            {
                fmc_rsv_s += " ";
                i ++;
            }

            string weight_unit_display = null;

            switch (unit)
            {
                case WeightUnit.KG:
                    weight_unit_display = "ALL WEIGHTS IN KG";
                    break;
                case WeightUnit.LB:
                    weight_unit_display = "ALL WEIGHTS IN LB";
                    break;
            }

            int fmc_reserve1 = 0;
            int fmc_reserve2 = 0;
            fmc_reserve1 = (alternate_display + final_rsv_display) / 1000;
            fmc_reserve2 = ((alternate_display + final_rsv_display) - fmc_reserve1 * 1000) / 100 + 1;
            if (fmc_reserve2 == 10)
            {
                fmc_reserve1 ++;
                fmc_reserve2 = 0;
            }
            //======================================================

            int time_cont = Convert.ToInt32((TimeToDest / 20));
            int time_TO = TimeToDest + time_cont + TimeExtra + TimeHold + TimeFinalRsv + TimeToAltn;
            int time_total = time_TO + TimeApu + TimeTaxi;

            StringBuilder OutputText = new StringBuilder();

            OutputText.Append(weight_unit_display + Environment.NewLine + Environment.NewLine + "              FUEL  TIME" + Environment.NewLine);
            OutputText.Append(trip_s + "  " + TimeFormat.MinToHHMM(TimeToDest) + Environment.NewLine);
            OutputText.Append(contingency_s + "  " + TimeFormat.MinToHHMM(time_cont) + Environment.NewLine);
            OutputText.Append(hold_s + "  " + TimeFormat.MinToHHMM(TimeHold) + Environment.NewLine);
            OutputText.Append(extra_s + "  " + TimeFormat.MinToHHMM(TimeExtra) + Environment.NewLine);
            OutputText.Append(alternate_s + "  " + TimeFormat.MinToHHMM(TimeToAltn) + Environment.NewLine);
            OutputText.Append(final_rsv_s + "  " + TimeFormat.MinToHHMM(TimeFinalRsv) + Environment.NewLine + Environment.NewLine);
            OutputText.Append(takeoff_s + "  " + TimeFormat.MinToHHMM(time_TO) + Environment.NewLine + Environment.NewLine);
            OutputText.Append(apu_s + "  " + TimeFormat.MinToHHMM(TimeApu) + Environment.NewLine);
            OutputText.Append(taxi_s + "  " + TimeFormat.MinToHHMM(TimeTaxi) + Environment.NewLine + Environment.NewLine);
            OutputText.Append(total_s + "  " + TimeFormat.MinToHHMM(time_total) + Environment.NewLine);
            OutputText.Append(fmc_rsv_s + fmc_reserve1 + "." + fmc_reserve2);

            return OutputText.ToString();

        }

        private string lineFormat(string item, int value)
        {
            return item.PadRight(LEFTPAD, ' ') + Convert.ToString(value).PadLeft(RIGHTPAD, ' ');
        }

    }
}
