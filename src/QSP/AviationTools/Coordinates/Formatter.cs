using System;
using static QSP.LibraryExtension.Types;

namespace QSP.AviationTools.Coordinates
{
    public static class Formatter
    {
        /// <summary>
        /// Given a waypoint ident, try to parse the following coordinate formats:
        /// Format7Letter, FormatDegMinNoSymbol. Then convert it to 5LetterFormat 
        /// if possible, if not possible, convert to DecimalFormat.
        /// 
        /// If the parsing step failed, returns the original string. 
        /// </summary>
        public static string TryTransformCoordinate(string item)
        {
            var parsers = List<Func<string, LatLon>>(
                Format7Letter.Parse, FormatDegMinNoSymbol.Parse);

            foreach (var p in parsers)
            {
                var coord = p(item);
                if (coord != null) return (coord.To5LetterFormat() ?? coord.ToDecimalFormat());
            }

            return item;
        }
    }
}
