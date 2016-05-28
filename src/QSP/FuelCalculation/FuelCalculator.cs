using System;
using System.Collections.Generic;
using QSP.MathTools.Interpolation;
using QSP.Common;
using QSP.LibraryExtension.JaggedArrays;

namespace QSP
{
    public class FuelCalculator
    {
        public double holdingFuelPerMinuteKg;
        public double maxFuelKg;
        public double taxiFuelPerMinKg;
        public double apuFuelPerMinKg;
        public double LandWeightTonAltn;
        public double LandWeightTonDest;
        public int TimeToDest;
        public int TimeToAltn;
        public FuelCalculationParameters Parameters;

        private string FuelTableSourceTxt;
        private string AirDisTimeOrigFile;
        private double[][] GroundToAirDisTable;
        private double[][] AirDisToFuelTable;
        private double[] airDis;
        private double[] LandingWt;
        private double fuelToAltn;
        private bool altnFuelComputed;
        private FlightTimeCalculator TimeCalc;

        public FuelCalculator(FuelCalculationParameters para)
        {
            Parameters = para;
            updateAltnLandWt();
            altnFuelComputed = false;

            switch (para.AC)
            {
                case Aircraft.B737600:
                    FuelTableSourceTxt = QspCore.PerfDB.Txt_737600txt;
                    AirDisTimeOrigFile = QspCore.PerfDB.Txt_737600_time_reqtxt;
                    holdingFuelPerMinuteKg = 38;
                    maxFuelKg = 20896;
                    taxiFuelPerMinKg = 12.2;
                    apuFuelPerMinKg = 1.8;
                    break;

                case Aircraft.B737700:
                    FuelTableSourceTxt = QspCore.PerfDB.Txt_737700txt;
                    AirDisTimeOrigFile = QspCore.PerfDB.Txt_737700_time_reqtxt;
                    holdingFuelPerMinuteKg = 38;
                    maxFuelKg = 20896;
                    taxiFuelPerMinKg = 12.2;
                    apuFuelPerMinKg = 1.8;
                    break;

                case Aircraft.B737800:
                    FuelTableSourceTxt = QspCore.PerfDB.Txt_737800txt;
                    AirDisTimeOrigFile = QspCore.PerfDB.Txt_737800_time_reqtxt;
                    holdingFuelPerMinuteKg = 38;
                    maxFuelKg = 20896;
                    taxiFuelPerMinKg = 12.2;
                    apuFuelPerMinKg = 1.8;
                    break;

                case Aircraft.B737900:
                    FuelTableSourceTxt = QspCore.PerfDB.Txt_737900txt;
                    AirDisTimeOrigFile = QspCore.PerfDB.Txt_737900_time_reqtxt;
                    holdingFuelPerMinuteKg = 38;
                    maxFuelKg = 20896;
                    taxiFuelPerMinKg = 12.2;
                    apuFuelPerMinKg = 1.8;
                    break;

                case Aircraft.B777200LR:
                    FuelTableSourceTxt = QspCore.PerfDB.Txt_777200LRtxt;
                    AirDisTimeOrigFile = QspCore.PerfDB.Txt_777200LR_time_reqtxt;
                    holdingFuelPerMinuteKg = 100;
                    maxFuelKg = 145752;
                    taxiFuelPerMinKg = 36;
                    apuFuelPerMinKg = 4;
                    break;

                case Aircraft.B777F:
                    FuelTableSourceTxt = QspCore.PerfDB.Txt_777Ftxt;
                    AirDisTimeOrigFile = QspCore.PerfDB.Txt_777F_time_reqtxt;
                    holdingFuelPerMinuteKg = 100;
                    maxFuelKg = 145752;
                    taxiFuelPerMinKg = 36;
                    apuFuelPerMinKg = 4;
                    break;
            }

            TimeCalc = new FlightTimeCalculator(AirDisTimeOrigFile);
            GroundToAirDisTable = loadGTATable(FuelTableSourceTxt);
            loadFuelTable(FuelTableSourceTxt);
        }

        private void updateDestLandWt()
        {
            LandWeightTonDest = 
                LandWeightTonAltn + 
                fuelToAltn + 
                (Parameters.HoldingMin * holdingFuelPerMinuteKg + Parameters.MissedAppFuelKg) / 1000 + Parameters.ExtraFuelKg / 1000;
        }

        private void updateAltnLandWt()
        {
            LandWeightTonAltn = (Parameters.ZfwKg + Parameters.FinalRsvMin * holdingFuelPerMinuteKg) / 1000;
        }

        private void loadFuelTable(string sourceTxt)
        {
            int p = sourceTxt.IndexOf("[Fuel]");
            int q = sourceTxt.IndexOf("[LDG WT]");

            string str = sourceTxt.Substring(p, q - p);
            string[] lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<string> validLine = new List<string>();

            foreach (var i in lines)
            {
                if (i.Length != 0 && char.IsDigit(i[0]))
                {
                    validLine.Add(i);
                }
            }

            int numCol = validLine[0].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var table = JaggedArray.Create<double[][]>(numCol - 1, validLine.Count);
            double[] airDisCol = new double[validLine.Count];
            double[] landingWtRow = new double[numCol];
            string[] numbers = null;

            for (int i = 0; i < validLine.Count; i++)
            {
                numbers = validLine[i].Split(new string[] {
                    " ",
                    "\t"
                }, StringSplitOptions.RemoveEmptyEntries);

                airDisCol[i] = Convert.ToDouble(numbers[0]);

                for (int j = 1; j <= numbers.Length - 1; j++)
                {
                    table[j - 1][i] = Convert.ToDouble(numbers[j]);
                }
            }

            airDis = airDisCol;
            AirDisToFuelTable = table;
            loadLandingWt(sourceTxt);

        }


        private void loadLandingWt(string sourceTxt)
        {
            string str = sourceTxt.Substring(sourceTxt.IndexOf("[LDG WT]"));
            string[] lines = str.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var i in lines)
            {
                if (i.Length > 0 && char.IsDigit(i[0]))
                {
                    string[] numbers = i.Split(new string[] {
                        " ",
                        "\t"
                    }, StringSplitOptions.RemoveEmptyEntries);
                    double[] wt = new double[numbers.Length];

                    for (int j = 0; j < wt.Length; j++)
                    {
                        wt[j] = Convert.ToDouble(numbers[j]);
                    }

                    LandingWt = wt;
                }
            }
        }

        private double[][] loadGTATable(string sourceTxt)
        {
            string gtaStr = sourceTxt.Substring(0, sourceTxt.IndexOf("[Fuel]"));
            string[] lines = gtaStr.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            List<string> validLine = new List<string>();

            foreach (var i in lines)
            {
                if (i.Length != 0 && char.IsDigit(i[0]))
                {
                    validLine.Add(i);
                }
            }

            int numCol = validLine[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var table = JaggedArray.Create<double[][]>(numCol, validLine.Count);
            string[] numbers = null;

            for (int i = 0; i < validLine.Count; i++)
            {
                numbers = validLine[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < numbers.Length; j++)
                {
                    table[j][i] = Convert.ToDouble(numbers[j]);
                }

            }

            return table;
        }

        private double getAirDis(double tailwind, double groundDis)
        {
            double[] wind = {
                -100,
                -80,
                -60,
                -40,
                -20,
                0,
                20,
                40,
                60,
                80,
                100
            };
            int rowCount = GroundToAirDisTable[0].Length;
            double[] grdDis = new double[rowCount];

            for (int i = 0; i < rowCount; i++)
            {
                grdDis[i] = GroundToAirDisTable[5][i];
            }
            return Interpolate2D.Interpolate(wind, grdDis, tailwind, groundDis, GroundToAirDisTable);
        }

        private double getFuelBurn(double landingWeight, double airDistance)
        {
            return Interpolate2D.Interpolate(LandingWt, airDis, landingWeight, airDistance, AirDisToFuelTable);
        }

        public double GetAltnFuelTon()
        {
            double airDistance = getAirDis(Parameters.AvgWindToAltn, Parameters.DisToAltn);
            TimeToAltn = TimeCalc.GetTimeMin(airDistance);
            fuelToAltn = getFuelBurn(LandWeightTonAltn, airDistance);
            altnFuelComputed = true;

            return fuelToAltn;
        }

        public double GetDestFuelTon()
        {
            if (altnFuelComputed == false)
            {
                GetAltnFuelTon();
            }

            updateDestLandWt();

            double airDistance = getAirDis(Parameters.AvgWindToDest, Parameters.DisToDest);
            TimeToDest = TimeCalc.GetTimeMin(airDistance);
            return getFuelBurn(LandWeightTonDest, airDistance);
        }

    }
}
