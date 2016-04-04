using System;
using QSP.Utilities;

namespace QSP.AviationTools
{
    public static class CoversionTools
    {
        public static double MachToTas(double mach, double altFt)
        {
            //knots
            return 39.0 * mach * Math.Sqrt(IsaTemp(altFt) + 273.0);
        }

        public static double IsaTemp(double AltFt)
        {
            ConditionChecker.Ensure
                <ArgumentOutOfRangeException>(AltFt <= 65000.0);

            if (AltFt <= 36000.0)
            {
                return 15.0 - 1.98 * AltFt / 1000.0;
            }
            else
            {
                return -56.5;
            }
        }

        public static double AltToPressureMb(double altFT)
        {
            //mb = hPa
            return Math.Pow((1.0 - altFT / 145366.45), (1.0 / 0.190284)) * 1013.25;
        }

        public static double PressureMbToAltFT(double pressMb)
        {
            return 145366.45 * (1.0 - Math.Pow((pressMb / 1013.25), 0.190284));
        }

        /// <summary>
        /// Returns the air density at specified altitude, in SI unit (kg/m^3). 
        /// </summary>
        public static double AirDensity(double altFt)
        {
            return AltToPressureMb(altFt) * 100 * 0.0288 / (8.314 * (273.0 + IsaTemp(altFt)));
        }

        public static double Ktas(double Kias, double altFt)
        {
            return Kias * Math.Sqrt(Constants.AirDensitySeaLevel / AirDensity(altFt));
        }

        public static string RwyIdentOppositeDir(string rwy)
        {
            int numPart = 0;
            char charPart;

            if (rwy.Length == 2)
            {
                numPart = Convert.ToInt32(rwy);
                charPart = ' ';
            }
            else
            {
                numPart = Convert.ToInt32(rwy.Substring(0, 2));
                charPart = rwy[2];
            }

            if (numPart >= 19)
            {
                numPart -= 18;
            }
            else
            {
                numPart += 18;
            }

            string numPartStr = numPart.ToString().PadLeft(2, '0');

            if (charPart == ' ')
            {
                return numPartStr;
            }
            else
            {
                return numPartStr + charPart;
            }

        }

        public static double PressureAltitudeFt(double elevationFt, double QNH)
        {
            return elevationFt + 30.0 * (1013.0 - QNH);
        }

        public static double ToCelsius(double temp)
        {
            return (temp - 32.0) * 5.0 / 9.0;
        }

        public static double ToFahrenheit(double temp)
        {
            return temp * 9.0 / 5.0 + 32;
        }
    }
}

