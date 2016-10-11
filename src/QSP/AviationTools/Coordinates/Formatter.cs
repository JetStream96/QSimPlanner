namespace QSP.AviationTools.Coordinates
{
    public static class Formatter
    {
        public static string TryTransformCoordinate(string item)
        {
            var coord = Format7Letter.Parse(item);
            if (coord == null) return item;
            return coord.To5LetterFormat() ?? coord.ToDecimalFormat();
        }
    }
}
