using System;

namespace QSP.LibraryExtension
{
    public static class TimeFormat
    {
        /// <summary>
        /// E.g. Input string: "1:43", output: 103.
        /// </summary>
        public static int HHColonMMToMin(string str)
        {
            try
            {
                string str1 = str.Substring(0, str.IndexOf(":"));
                string str2 = str.Substring(str.IndexOf(":") + 1);
                return Convert.ToInt32(str1) * 60 + Convert.ToInt32(str2);
            }
            catch
            {
                throw new ArgumentException("Bad format.");
            }
        }

        public static int HHMMToMin(string str)
        {
            //e.g. 0143 ---> 103
            try
            {
                string str1 = str.Substring(0, 2);
                string str2 = str.Substring(2);
                return Convert.ToInt32(str1) * 60 + Convert.ToInt32(str2);
            }
            catch
            {
                throw new ArgumentException("Bad format.");
            }
        }

        public static string MinToHHMM(int min)
        {
            //return a value with exactly 4 digits
            if (min >= 60 * 100 || min < 0)
            {
                throw new ArgumentException("Bad format.");
            }

            int h = min / 60;
            int m = min % 60;
            return h.ToString().PadLeft(2, '0') + m.ToString().PadLeft(2, '0');
        }

    }

}

