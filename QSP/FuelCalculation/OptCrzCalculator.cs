using QSP.AviationTools;
using QSP.Core;
using System;
using QSP.MathTools.Interpolation;

namespace QSP
{
    public class OptCrzCalculator
    {        
        private double[] weight;  //1000KG       
        private double[] alt;    //ft

        private Tuple<double, double, double> CrzTas;

        private void loadFromTxt(string s)
        {
            string[] lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            bool wtUnitIsKG = false;

            if (lines[0].Substring(0, 2) == "KG")
            {
                wtUnitIsKG = true;
            }
            else
            {
                wtUnitIsKG = false;
            }

            weight = new double[lines.Length - 1];
            alt = new double[lines.Length - 1];

            string[] x = null;

            for (int i = 1; i < lines.Length ; i++)
            {
                x = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                weight[i - 1] = Convert.ToDouble(x[0]) * (wtUnitIsKG ? 1.0 : Constants.LB_KG);
                alt[i - 1] = Convert.ToDouble(x[1]);
            }
        }
        
        public OptCrzCalculator(Aircraft Ac)
        {
            string source = null;

            switch (Ac)
            {
                case Aircraft.B737600:
                    source = QspCore.PerfDB.B737600_OptCrzAlt;
                    CrzTas = new Tuple<double, double, double>(250, 280, 0.78);
                    break;

                case Aircraft.B737700:
                    source = QspCore.PerfDB.B737700_OptCrzAlt;
                    CrzTas = new Tuple<double, double, double>(250, 280, 0.78);
                    break;

                case Aircraft.B737800:
                    source = QspCore.PerfDB.B737800_OptCrzAlt;
                    CrzTas = new Tuple<double, double, double>(250, 280, 0.78);
                    break;

                case Aircraft.B737900:
                    source = QspCore.PerfDB.B737900_OptCrzAlt;
                    CrzTas = new Tuple<double, double, double>(250, 280, 0.78);
                    break;

                case Aircraft.B777200LR:
                    source = QspCore.PerfDB.B777200LR_OptCrzAlt;
                    CrzTas = new Tuple<double, double, double>(250, 310, 0.84);
                    break;

                case Aircraft.B777F:
                    source = QspCore.PerfDB.B777F_OptCrzAlt;
                    CrzTas = new Tuple<double, double, double>(250, 310, 0.84);
                    break;
            }
            loadFromTxt(source);
        }

        public double OptimumAltitude(double wt)
        {
            //FT
            return Interpolate1D.Interpolate(weight, alt, wt);
        }

        public double DisLimitedAlt(double disNm)
        {
            //FT
            return disNm / 2 * 1000 / 3;
        }

        public double ActualCrzAlt(double wt, double disNm)
        {
            //FT
            return Math.Min(OptimumAltitude(wt), DisLimitedAlt(disNm));
        }

        public double CruiseTas(double altFT)
        {
            //KTS
            if (altFT <= 10000)
            {
                return CrzTas.Item1;
            }
            return Math.Min(CoversionTools.MachToTas(CrzTas.Item3, altFT), CoversionTools.Ktas(CrzTas.Item2, altFT));
        }
    }
}
