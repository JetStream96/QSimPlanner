using System.Collections.Generic;
using QSP.LibraryExtension;

namespace QSP.RouteFinding
{
    public static class FixTypes
    {
        // This lists all kinds of fixes with coordinates.
        // Not all fixes have lat/lon. For example, in SIDs,
        // a fix may be "maintain heading 090".
        //
        // These may also be included:
        // "RF", "CD", "FA", "FC", "FM", "VD", "PI", "HF", "HA", "HM"
        // But it seems these are not used in Aerosoft Airbus NavData.
        //
        private static readonly HashSet<string> correctFixType =
            new[] { "IF", "DF", "TF", "FD", "CF", "AF" }.ToHashSet();

        /// <summary>
        /// Determines whether the fix has a lat/lon property.
        /// </summary>
        /// <param name="fixType">Two letter code for the fix.</param>
        public static bool HasCorrds(string fixType)
        {
            return correctFixType.Contains(fixType);
        }
    }
}
