using System.Linq;

namespace QSP.RouteFinding
{
    public static class RwyIdent
    {
        /// <summary>
        /// Determines whether the input string is a valid rwy identifier.
        /// </summary>
        public static bool IsRwyIdent(string str)
        {
            if (str.Length == 2) return ValidRwyNum(str);
            if (str.Length == 3) return "LRC".Contains(str[2]) && ValidRwyNum(str.Substring(0, 2));
            return false;
        }

        private static bool ValidRwyNum(string s)
        {
            int rwyNum;
            return int.TryParse(s, out rwyNum) && 0 < rwyNum && rwyNum <= 36;
        }
    }
}

