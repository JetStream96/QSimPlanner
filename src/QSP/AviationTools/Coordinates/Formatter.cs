namespace QSP.AviationTools.Coordinates
{
    public static class Formatter
    {
        public static void TransformCoordinates(string[] item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                var coord = Format7Letter.Parse(item[i]);

                if (coord != null)
                {
                    item[i] = coord.To5LetterFormat() ?? 
                        coord.ToDecimalFormat();
                }
            }
        }
    }
}
