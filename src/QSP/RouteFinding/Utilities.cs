namespace QSP.RouteFinding
{
    public static class Utilities
    {
        private static readonly string[] correctFixType =
            { "IF", "DF", "TF", "FD", "CF" };
        //, "AF", "RF", "CD", "FA", "FC", "FM", "VD", "PI", "HF", "HA", "HM"}
        //this lists all kinds of fixes with coordinates located in t(2) and t(3)

        /// <summary>
        /// Determines whether the input string is a valid rwy identifier.
        /// </summary>
        public static bool IsRwyIdent(string str)
        {
            int rwyNum;

            if (str.Length == 2)
            {
                return
                    int.TryParse(str, out rwyNum) &&
                    (rwyNum > 0 && rwyNum <= 36);
            }
            else if (str.Length == 3)
            {
                return
                    (str[2] == 'L' || str[2] == 'R' || str[2] == 'C') &&
                    int.TryParse(str.Substring(0, 2), out rwyNum) &&
                    (rwyNum > 0 && rwyNum <= 36);
            }

            return false;
        }

        public static bool HasCorrds(string fixType)
        {
            foreach (var i in correctFixType)
            {
                if (i == fixType)
                {
                    return true;
                }
            }
            return false;
        }
    }
}

