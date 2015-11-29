using System;

namespace QSP.AviationTools
{

    public static class LatLonConversion
    {

        /// <summary>
        /// Determine whether the string has the format like 36N170W.
        /// </summary>
        public static bool Is7DigitFormat(string item)
        {
            if (item.Length == 7 &&
                char.IsDigit(item[0]) && char.IsDigit(item[1]) && char.IsDigit(item[3]) && char.IsDigit(item[4]) &&
                char.IsDigit(item[5]) &&
                (item[2] == 'N' || item[2] == 'S') &&
                (item[6] == 'E' || item[6] == 'W'))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Input examples: 36N170W 34N080E
        /// Output examples: 36N70   3480E
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static string Convert7DigitTo5Digit(string item)
        {
            try
            {
                if (item[6] == 'E')
                {
                    if (item[2] == 'N')
                    {
                        return get5Digit(item, 'E');
                    }
                    else
                    {
                        return get5Digit(item, 'S');
                    }
                }
                else
                {
                    if (item[2] == 'N')
                    {
                        return get5Digit(item, 'N');
                    }
                    else
                    {
                        return get5Digit(item, 'W');
                    }
                }
            }
            catch
            {
                throw new ArgumentException("Wrong format.");
            }
        }

        private static string get5Digit(string item, char c)
        {
            if (item[3] == '0')
            {
                return appendEnd(item, c);
            }
            else
            {
                return insertMiddle(item, c);
            }
        }

        private static string appendEnd(string item, char c)
        {
            char[] result = { item[0], item[1], item[4], item[5], c };
            return new string(result);
        }

        private static string insertMiddle(string item, char c)
        {
            char[] result = { item[0], item[1], c, item[4], item[5] };
            return new string(result);
        }

    }

}

