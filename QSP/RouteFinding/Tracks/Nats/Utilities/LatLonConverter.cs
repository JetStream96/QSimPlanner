namespace QSP.RouteFinding.Tracks.Nats.Utilities
{
    public static class LatLonConverter
    {
        /// <summary>
        /// Sample input : "54/20", "5530/20". Sample output : "5420N","55.5N20W".
        /// If the input is not the correct format, null is returned.
        /// </summary>        
        public static string NatsLatLonToIdent(string s)
        {
            int x = s.IndexOf('/');

            if (x < 0)
            {
                return null;
            }

            string LatS = s.Substring(0, x);
            throw new System.Exception();//TODO:
        }
        
        public static bool IsNatsLatLonFormat(string s)
        {
            int x = s.IndexOf('/');



            return (s.Length == 5 &&
                    s[2] == '/' &&
                    char.IsDigit(s[0]) &&
                    char.IsDigit(s[1]) &&
                    char.IsDigit(s[3]) &&
                    char.IsDigit(s[4]));
        }

        private static void tryConvertNatsLatLon(string[] wpts)
        {
            for (int i = 0; i < wpts.Length; i++)
            {
                if (IsNatsLatLonFormat(wpts[i]))
                {
                    wpts[i] = NatsLatLonToIdent(wpts[i]);
                }
            }
        }
    }
}
