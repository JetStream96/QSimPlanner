using QSP.AviationTools;
using QSP.WindAloft;
using System.Text.RegularExpressions;

namespace QSP.Metar
{
    public static class ParaExtractor
    {
        // TODO: Replace with regex.

        /// <summary>
        /// E.g. matches "VRB", "02013KT", or "14007MPS".
        /// If not found, returns null.
        /// </summary>
        public static Wind GetWind(string metar)
        {
            if (Regex.Match(metar, "\\bVRB\\d{1,3}(KTS?|MPS)\\b").Success)
            {
                return new Wind(0.0, 0.0);
            }

            var match = Regex.Match(metar, "\\b\\d{3}/?\\d{1,3}(KTS?|MPS)\\b");

            if (match.Success)
            {
                var val = match.Value;
                int direction = int.Parse(val.Substring(0, 3));
                double speed = int.Parse(
                    Regex.Match(val.Substring(3), "^\\d{1,3}").Value);

                if (val.Contains("MPS"))
                {
                    speed /= Constants.KT_MPS;
                }
                return new Wind(direction, speed);
            }

            return null;
        }

        /// <summary>
        /// Matches 12/09, 01/M01, M04/M06, etc.
        /// Returns int.Min if no match is found.
        /// </summary>
        public static int GetTemp(string metar)
        {
            var match = Regex.Match(metar, "\\bM?\\d{1,3}/M?\\d{1,3}\\b");

            if (match.Success)
            {
                var val = match.Value;

                if (val[0] == 'M')
                {
                    return -int.Parse(val.Substring(1, val.IndexOf('/') - 1));
                }
                else
                {
                    return int.Parse(val.Substring(0, val.IndexOf('/')));
                }
            }
            else
            {
                return int.MinValue;
            }
        }

        /// <summary>
        /// Matches Q1013, A3000.
        /// </summary>
        /// <param name="metar"></param>
        /// <returns></returns>
        public static double GetQNH(string metar)
        {

        }

        public static string PressInfo(string metar)
        {
            //will return, e.g. 
            //return "NA" if nothing is found
            bool meetCondition = false;
            try
            {
                for (int i = 0; i <= metar.Length - 5; i++)
                {
                    meetCondition = true;
                    if (metar[i] == 'Q' || metar[i] == 'A')
                    {
                        for (int j = 1; j <= 4; j++)
                        {
                            if (!char.IsDigit(metar[i + j]))
                            {
                                meetCondition = false;
                                break;
                            }
                        }

                        if (meetCondition == true)
                        {
                            return metar.Substring(i, 5);
                        }
                    }
                }
            }
            catch
            {
                return "NA";
            }
            return "NA";
        }

        public class PressureSetting
        {
            public enum Unit
            {
                Mb,
                inHg
            }

            public Unit PressUnit { get; private set; }
            public double Value { get; private set; }

            public PressureSetting(Unit PressUnit, double Value)
            {
                this.PressUnit = PressUnit;
                this.Value = Value;
            }
        }
    }
}
