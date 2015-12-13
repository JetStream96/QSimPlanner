using System;

namespace QSP.Core
{
    public static class EnumConversionTools
    {
        public static string AircraftToString(Aircraft ac)
        {
            switch (ac)
            {
                case Aircraft.B737600:
                    return "737-600";

                case Aircraft.B737700:
                    return "737-700";

                case Aircraft.B737800:
                    return "737-800";

                case Aircraft.B737900:
                    return "737-900";

                case Aircraft.B777200LR:
                    return "777-200LR";

                case Aircraft.B777F:
                    return "777F";
            }
            throw new ArgumentOutOfRangeException("Invalid aircraft.");
        }

        public static Aircraft StringToAircraft(string item)
        {
            switch (item)
            {
                case "737-600":
                    return Aircraft.B737600;

                case "737-700":
                    return Aircraft.B737700;

                case "737-800":
                    return Aircraft.B737800;

                case "737-900":
                    return Aircraft.B737900;

                case "777-200LR":
                    return Aircraft.B777200LR;

                case "777F":
                    return Aircraft.B777F;
            }
            throw new ArgumentOutOfRangeException("Invalid aircraft.");
        }

        public static string WeightUnitToString(WeightUnit item)
        {
            return item == WeightUnit.KG ? "KG" : "LB";
        }

        public static WeightUnit StringToWeightUnit(string item)
        {
            switch (item)
            {
                case "KG":
                    return WeightUnit.KG;
                case "LB":
                    return WeightUnit.LB;
            }
            throw new ArgumentOutOfRangeException("Invalid weight unit.");
        }
    }
}
