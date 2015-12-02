using System;

namespace QSP.LibraryExtension
{
    public static class TimeFormat
    {
        public static int HH_Colon_MMToMin(string str)
        {
            //e.g. 1:43 ---> 103
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
                return "";
            }
            int h = min / 60;
            int m = min - 60 * h;

            string HH = null;
            string MM = null;

            if (h <= 9)
            {
                HH = "0" + h.ToString();
            }
            else
            {
                HH = h.ToString();
            }

            if (m <= 9)
            {
                MM = "0" + m.ToString();
            }
            else
            {
                MM = m.ToString();
            }

            return HH + MM;
        }

    }

}

