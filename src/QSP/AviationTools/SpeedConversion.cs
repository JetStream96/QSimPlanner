using QSP.MathTools;
using static System.Math;
using static QSP.AviationTools.ConversionTools;

namespace QSP.AviationTools
{
    public static class SpeedConversion
    {
        // Here are some important type of airspeeds:
        // IAS, TAS, EAS, CAS, Mach number
        //
        // Since CAS is IAS with corrections due to angle of attack, position 
        // of pitot tube and gauge errors, the two values can be used 
        // interchangably for our calculations.

        public static double TasKnots(double mach, double altFt)
        {
            return 39.0 * mach * Sqrt(IsaTemp(altFt) + 273.0);
        }
        
        public static double KtasToMach(double ktas, double altFt)
        {
            return ktas / (39.0 * Sqrt(IsaTemp(altFt) + 273.0));
        }

        public static double KtasToKcas(double ktas, double altFt)
        {
            return Kcas(KtasToMach(ktas, altFt), altFt);
        }

        public static double Ktas(double kcas, double altFt)
        {
            double eas = KcasToKeas(kcas, altFt);
            return eas * Sqrt(AirDensity(0.0) / AirDensity(altFt));
        }

        public static double KcasToKeas(double kcas, double altFt)
        {
            // The equation are:
            // (1) Keas * ConversionFactor(delta, mach) = Kcas
            // (2) Keas = mach * standardSoundSpeedKnots * Sqrt(delta)
            // Note: Equation (2) is implemented in method MachToKeas.

            double eas = kcas;
            var delta = Delta(altFt);
            double mach;
            const int iterationCount = 5;

            // Use iteration to improve accuracy.
            for (int i = 0; i < iterationCount; i++)
            {
                mach = MachNumber(eas, delta);
                eas = kcas / ConversionFactor(delta, mach);
            }

            return eas;
        }

        public static double Delta(double altitudeFt)
        {
            return PressureMb(altitudeFt) / PressureMb(0.0);
        }

        public static double ConversionFactor(double delta, double mach)
        {
            return 1.0 + 1.0 / 8.0 * (1.0 - delta) * mach * mach +
                    3.0 / 640.0 * (1.0 - 10.0 * delta + 9 * delta * delta)
                    * Numbers.Pow(mach, 4);
        }

        /// <summary>
        /// Returns the calibrated airspeed in knots.
        /// </summary>
        public static double Kcas(double mach, double altFt)
        {
            var delta = Delta(altFt);
            var eas = MachToKeas(mach, delta);
            return eas * ConversionFactor(delta, mach);
        }

        /// <summary>
        /// Returns the equivalent airspeed in knots.
        /// </summary>
        public static double MachToKeas(double mach, double delta)
        {
            const double standardSoundSpeedKnots = 661.47;
            return mach * standardSoundSpeedKnots * Sqrt(delta);
        }

        public static double MachNumber(double keas, double delta)
        {
            const double standardSoundSpeedKnots = 661.47;
            return keas / standardSoundSpeedKnots / Sqrt(delta);
        }
    }
}
