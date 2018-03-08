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
            var coord = Format7Letter.Parse(item);

            if (coord == null)
            {
                coord = FormatDegMinNoSymbol.Parse(item);
                if (coord == null) return item;
            }

            return coord.To5LetterFormat() ?? coord.ToDecimalFormat();
        }
    }
}
