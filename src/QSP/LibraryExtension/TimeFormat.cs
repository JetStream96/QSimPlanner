using QSP.Utilities;
using System;
using static QSP.Utilities.ConditionChecker;

namespace QSP.LibraryExtension
{
    public static class TimeFormat
    {
        /// <summary>
        /// E.g. Input string: "1:43", output: 103.
        /// </summary>
        public static int HourColonMinToMin(string s)
        {
            int colonIndex = s.IndexOf(':');
            Ensure<ArgumentException>(colonIndex >= 0);
            return StringsToMin(
                s.Substring(0, colonIndex), s.Substring(colonIndex + 1));
        }
        
        //e.g. 0143 ---> 103
        public static int HHMMToMin(string s)
        {
            ConditionChecker.Ensure<ArgumentException>(s.Length == 4);
            return StringsToMin(s.Substring(0, 2), s.Substring(2));
        }

        private static int StringsToMin(string hour, string min)
        {
            int h = int.Parse(hour);
            int m = int.Parse(min);

            ConditionChecker.Ensure<ArgumentException>(
                h >= 0 && m >= 0 && m <= 60);

            return 60 * h + m;
        }

        public static string MinToHHMM(int min)
        {
            //return a value with exactly 4 digits
            ConditionChecker.Ensure<ArgumentOutOfRangeException>(
                0 <= min && min < 60 * 100);

            int h = min / 60;
            int m = min % 60;
            return h.ToString().PadLeft(2, '0') + m.ToString().PadLeft(2, '0');
        }
    }
}
