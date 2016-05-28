using System;

namespace QSP.Utilities.Units
{
    public static class Conversions
    {
        // TODO: ???
        public static string ToString(this WeightUnit item)
        {
            return item == WeightUnit.KG ? "KG" : "LB";
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

                default:
                    throw new ArgumentOutOfRangeException(
                        "Invalid weight unit.");
            }
        }
    }
}
