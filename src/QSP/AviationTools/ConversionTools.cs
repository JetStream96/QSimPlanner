using System;
using static QSP.Utilities.ExceptionHelpers;

namespace QSP.AviationTools
{
    public static class ConversionTools
    {        
        /// <summary>
        /// Returns the temperature at given altitude, using ICAO standard 
        /// atmosphere. Only allows altitudes less than or equal to 65000 feet.
        /// </summary>
        public static double IsaTemp(double AltFt)
        {
            Ensure<ArgumentOutOfRangeException>(AltFt <= 65000.0);

            var temp = 15.0 - 1.98 * AltFt / 1000.0;
            if (temp >= -56.5) return temp;

            return -56.5;
        }

        public static double PressureMb(double altitudeFeet)
        {
            //mb = hPa
            return Math.Pow(1.0 - altitudeFeet / 145366.45, 1.0 / 0.190284) 
                * 1013.25;
        }

        public static double PressureMbToAltFt(double pressMb)
        {
            return 145366.45 * (1.0 - Math.Pow((pressMb / 1013.25), 0.190284));
        }

        /// <summary>
        /// Returns the air density at specified altitude, in SI unit (kg/m^3). 
        /// </summary>
        public static double AirDensity(double altFt)
        {
            return PressureMb(altFt) * 100 * 0.0288 /
                (8.314 * (273.15 + IsaTemp(altFt)));
        }
        
        public static double PressureAltitudeFt(double elevationFt, double QNH)
        {
            return elevationFt + 30.0 * (1013.25 - QNH);
        }

        public static double ToCelsius(double temp)
        {
            return (temp - 32.0) * 5.0 / 9.0;
        }

        public static double ToFahrenheit(double temp)
        {
            return temp * 9.0 / 5.0 + 32.0;
        }
    }
}

