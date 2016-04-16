using System;

namespace QSP.AviationTools.Coordinates
{
    public static class Utilities
    {
        public static bool tryConvertInt(double d, ref int result)
        {
            double x = Math.Round(d);

            if (Math.Abs(x - d) < Constants.LatLon_TOLERENCE)
            {
                result = (int)x;
                return true;
            }
            return false;
        }
        
        public static void ConvertTo5LetterFormat(string[] item)
        {
            for (int i = 0; i < item.Length; i++)
            {
                LatLon coord;

                if (Format7Letter.TryReadFrom7LetterFormat(item[i], out coord))
                {
                    try
                    {
                        item[i] = coord.To5LetterFormat();
                    }
                    catch
                    {
                        item[i] = coord.ToDecimalFormat();
                    }
                }
            }
        }
    }
}
