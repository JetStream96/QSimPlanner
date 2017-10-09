using QSP.LibraryExtension;
using CommonLibrary.LibraryExtension.Sets;

namespace QSP.RouteFinding
{
    public static class FixTypes
    {
        // This lists all kinds of fixes with coordinates.
        // Not all fixes have lat/lon. For example, in SIDs,
        // a fix may be "maintain heading 090".
        //
        // The fix types with coordinates can be analyzed with FixTypeAnalyzer in this solution.
        //
        public static readonly IReadOnlySet<string> FixTypesWithCoords =
            new[] { "IF", "DF", "TF", "FD", "CF", "AF", "FA", "HA", "FM", "FC", "RF", "HM", "HF" }
            .ToReadOnlySet();

        /// <summary>
        /// Determines whether the fix has a lat/lon property.
        /// </summary>
        /// <param name="fixType">Two letter code for the fix.</param>
        public static bool HasCorrds(string fixType)
        {
            return FixTypesWithCoords.Contains(fixType);
        }
    }
}
