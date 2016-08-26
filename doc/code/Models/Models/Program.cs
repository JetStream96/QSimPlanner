using System;
using System.IO;
using System.Text;
using static System.Math;

namespace Model1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change this variable to use model 1, 2 or 3.
            Func<double, double, double> model = B737800.GetFuelTonModel2;

            double[] distances =
            {
                50,
                100,
                150,
                200,
                250,
                300,
                350,
                400,
                450,
                500,
                1000,
                1400,
                1800,
                2200,
                2600,
                3000,
                3400,
                3800,
                4200,
                4600,
                5000,
            };

            var sb = new StringBuilder();

            foreach (var dis in distances)
            {
                string s = "";

                for (int wt = 40; wt <= 70; wt += 5)
                {
                    s += model(wt, dis).ToString("F2") + ", ";
                }

                sb.AppendLine(s);
            }

            File.WriteAllText("out.txt", sb.ToString());
        }
    }

    static class B737800
    {
        public static double GetFuelTonModel1(
            double landingWeightTon, double distanceNm)
        {
            double distance = 0.0;
            double weightTon = landingWeightTon;
            const double deltaTimeHour = 0.01;

            while (distance < distanceNm)
            {
                double ff = FuelFlowTonPerHour(weightTon);
                double tas = TasKnots(weightTon);

                distance += tas * deltaTimeHour;
                weightTon += ff * deltaTimeHour;
            }

            return weightTon - landingWeightTon;
        }

        public static double GetFuelTonModel2(
           double landingWeightTon, double distanceNm)
        {
            double distance = 0.0;
            double weightTon = landingWeightTon;
            const double deltaTimeHour = 0.01;

            distance += DescendDistanceNm(weightTon);

            weightTon += DescendFuelFlow(weightTon) *
                DescendTimeHour(weightTon);

            while (distance + ClimbDistanceNm(weightTon) < distanceNm)
            {
                double ff = FuelFlowTonPerHour(weightTon);
                double tas = TasKnots(weightTon);

                distance += tas * deltaTimeHour;
                weightTon += ff * deltaTimeHour;
            }

            weightTon += ClimbFuelFlow(weightTon) *
                ClimbTimeHour(weightTon);

            return weightTon - landingWeightTon;
        }

        public static double GetFuelTonModel3(
           double landingWeightTon, double distanceNm)
        {
            double distance = 0.0;
            double weightTon = landingWeightTon;
            const double deltaTimeHour = 0.01;

            if (distanceNm < DistanceLimit(weightTon))
            {
                return FuelTonDistanceLimited(landingWeightTon, distanceNm);
            }

            distance += DescendDistanceNm(weightTon);

            weightTon += DescendFuelFlow(weightTon) *
                DescendTimeHour(weightTon);

            while (distance + ClimbDistanceNm(weightTon) < distanceNm)
            {
                double ff = FuelFlowTonPerHour(weightTon);
                double _tas = TasKnots(weightTon);
                double tas = Util.Ktas(IasKnots((weightTon)),
                    OptimumAltitudeFeet(weightTon));

                distance += tas * deltaTimeHour;
                weightTon += ff * deltaTimeHour;
            }

            weightTon += ClimbFuelFlow(weightTon) *
                ClimbTimeHour(weightTon);

            return weightTon - landingWeightTon;
        }

        private static double FuelTonDistanceLimited(
            double landingWeightTon, double distanceNm)
        {
            var wt = landingWeightTon;
            double alt = DistanceLimitedAltitude(wt, distanceNm);
            return ClimbTimeHour(wt, alt) * ClimbFuelFlow(wt) +
                DescendTimeHour(wt, alt) * DescendFuelFlow(wt);
        }

        private static double DistanceLimitedAltitude(
            double landingWeightTon, double distanceNm)
        {
            double alt = OptimumAltitudeFeet(landingWeightTon);

            while (ClimbDistanceNm(landingWeightTon, alt) +
                DescendDistanceNm(landingWeightTon, alt) > distanceNm)
            {
                alt -= 100;
            }

            return alt;
        }

        private static double DistanceLimit(double landingWeightTon)
        {
            return DescendDistanceNm(landingWeightTon) +
                ClimbDistanceNm(landingWeightTon);
        }

        private static double OptimumAltitudeFeet(double grossWeightTon)
        {
            var opt = 56057.0 - grossWeightTon * 305.71;
            return Min(41000.0, opt);
        }

        private static double ClimbDistanceNm(double grossWeightTon)
        {
            var alt = OptimumAltitudeFeet(grossWeightTon);
            return ClimbDistanceNm(grossWeightTon, alt);
        }

        private static double ClimbDistanceNm(
            double grossWeightTon, double altitudeFt)
        {
            var grad = ClimbGradient(grossWeightTon);
            return (altitudeFt / 6076.11549) / grad;
        }

        private static double ClimbTimeHour(double grossWeightTon)
        {
            return ClimbTimeHour(grossWeightTon,
                OptimumAltitudeFeet(grossWeightTon));
        }

        private static double ClimbTimeHour(
            double grossWeightTon, double altitudeFt)
        {
            var speed = Util.Ktas(280.0, 0.5 * altitudeFt);
            return ClimbDistanceNm(grossWeightTon, altitudeFt) / speed;
        }

        private static double ClimbFuelFlow(double grossWeightTon)
        {
            return (5125.0 + (grossWeightTon - 40.0) / 45.0 *
                (5648.0 - 5125.0)) / 1000.0;
        }

        private static double ClimbGradient(double grossWeightTon)
        {
            double grad40ton = 41000.0 / 6076.11549 / 75.0;
            double grad85ton = 30300.0 / 6076.11549 / 116.0;

            return grad40ton + (grossWeightTon - 40.0) / 45.0 *
                (grad85ton - grad40ton);
        }

        private static double DescendDistanceNm(double grossWeightTon)
        {
            var alt = OptimumAltitudeFeet(grossWeightTon);
            return DescendDistanceNm(grossWeightTon, alt);
        }

        private static double DescendDistanceNm(
            double grossWeightTon, double altitudeFt)
        {
            var grad = DescendGradient(grossWeightTon);
            return (altitudeFt / 6076.11549) / grad;
        }

        private static double DescendTimeHour(double grossWeightTon)
        {
            return DescendTimeHour(grossWeightTon,
                OptimumAltitudeFeet(grossWeightTon));
        }

        private static double DescendTimeHour(
            double grossWeightTon, double altitudeFt)
        {
            var speed = Util.Ktas(280.0, 0.5 * altitudeFt);
            return DescendDistanceNm(grossWeightTon, altitudeFt) / speed;
        }

        private static double DescendFuelFlow(double grossWeightTon)
        {
            return (785.0 + (grossWeightTon - 40.0) / 45.0 *
                (865.0 - 785.0)) / 1000.0;
        }

        private static double DescendGradient(double grossWeightTon)
        {
            double grad40ton = 41000.0 / 6076.11549 / 101.0;
            double grad85ton = 30300.0 / 6076.11549 / 107.0;

            return grad40ton + (grossWeightTon - 40.0) / 45.0 *
                (grad85ton - grad40ton);
        }

        private static double FuelFlowTonPerHour(double grossWeightTon)
        {
            // 40 ton -> 1626.0 kg/hr
            // 85 ton -> 3183.8 kg/hr
            return (1626.0 +
                (grossWeightTon - 40.0) / 45.0 * (3183.8 - 1626.0))
                / 1000.0;
        }

        private static double TasKnots(double grossWeightTon)
        {
            // 40 ton -> 430 knots
            // 85 ton -> 464 knots
            return 430.0 + (grossWeightTon - 40.0) / 45.0 * 34.0;
        }

        private static double IasKnots(double grossWeightTon)
        {
            return 1.8028 * grossWeightTon + 143.8652;
        }
    }

    class Util
    {
        public const double AirDensitySeaLevel = 101325 * 0.0288 / (8.314 * 288);

        public static double IsaTemp(double AltFt)
        {
            if (AltFt > 65000.0) throw new ArgumentOutOfRangeException();

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
            return Pow(1.0 - altFT / 145366.45, 1.0 / 0.190284) * 1013.25;
        }

        public static double AirDensity(double altFt)
        {
            return AltToPressureMb(altFt) * 100 * 0.0288 /
                (8.314 * (273.0 + IsaTemp(altFt)));
        }

        public static double Ktas(double kias, double altFt)
        {
            double eas = EquivalentAirspeedKnots(kias, altFt);
            return eas * Sqrt(AirDensitySeaLevel / AirDensity(altFt));
        }

        public static double EquivalentAirspeedKnots(double cas, double altFt)
        {
            double eas = cas;
            var delta = Delta(altFt);
            double mach = 0.0;

            // Use iteration.
            for (int i = 0; i < 5; i++)
            {
                mach = MachNumber(eas, delta);
                eas = cas / ConversionFactor(delta, mach);
            }

            return eas;
        }

        private static double Delta(double altitudeFt)
        {
            return AltToPressureMb(altitudeFt) / AltToPressureMb(0.0);
        }

        private static double ConversionFactor(double delta, double mach)
        {
            return 1.0 + 1.0 / 8.0 * (1.0 - delta) * mach * mach +
                    3.0 / 640.0 * (1.0 - 10.0 * delta + 9 * delta * delta)
                    * Pow(mach, 4.0);
        }

        public static double CasKnots(double mach, double altFt)
        {
            var delta = Delta(altFt);
            var eas = EasKnots(mach, delta);
            return eas * ConversionFactor(delta, mach);
        }

        public static double EasKnots(double mach, double delta)
        {
            const double standardSoundSpeedKnots = 661.47;
            return mach * standardSoundSpeedKnots * Sqrt(delta);
        }

        public static double MachNumber(double eas, double delta)
        {
            const double standardSoundSpeedKnots = 661.47;
            return eas / standardSoundSpeedKnots / Sqrt(delta);
        }
    }
}
