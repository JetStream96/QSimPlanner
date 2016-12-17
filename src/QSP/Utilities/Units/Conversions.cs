using System;

namespace QSP.Utilities.Units
{
    public static class Conversions
    {
        public static string WeightUnitToString(WeightUnit item)
        {
            return item == WeightUnit.KG ? "KG" : "LB";
        }

        public static WeightUnit StringToWeightUnit(string item)
        {
            if (item == "KG") return WeightUnit.KG;
            if (item == "LB") return WeightUnit.LB;
            throw new ArgumentOutOfRangeException("Invalid weight unit.");
        }
    }
}
