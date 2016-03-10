using QSP.AviationTools;
using QSP.WindAloft;
using System.Text.RegularExpressions;

namespace QSP.Metar
{
    public static class ParaExtractor
    {
        /// <summary>
        /// E.g. matches "VRB", "02013KT", or "14007MPS".
        /// If not found, returns null.
        /// </summary>
        public static Wind GetWind(string metar)
        {
            if (Regex.Match(metar, @"\sVRB\d{1,3}(KTS?|MPS)\s").Success)
            {
                return new Wind(0.0, 0.0);
            }

            var match = Regex.Match(metar, @"\s\d{3}/?\d{1,3}(KTS?|MPS)\s");

            if (match.Success)
            {
                var val = match.Value;
                int direction = int.Parse(val.Substring(1, 3));
                double speed = int.Parse(
                    Regex.Match(val.Substring(4), @"^\d{1,3}").Value);

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
            var match = Regex.Match(metar, @"\sM?\d{1,3}/M?\d{1,3}\s");

            if (match.Success)
            {
                var val = match.Value;

                if (val[0] == 'M')
                {
                    return -int.Parse(val.Substring(2, val.IndexOf('/') - 2));
                }
                else
                {
                    return int.Parse(val.Substring(1, val.IndexOf('/') - 1));
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
        public static PressureSetting GetPressure(string metar)
        {
            var match = Regex.Match(metar, @"\s[AQ]\d{4}\s");

            if (match.Success)
            {
                var val = match.Value;
                return new PressureSetting(
                    val.Contains("A") ? PressureUnit.inHg : PressureUnit.Mb,
                    double.Parse(val.Substring(2, 4)) / 100.0);
            }

            return null;
        }

        public class PressureSetting
        {
            public PressureUnit PressUnit { get; private set; }
            public double Value { get; private set; }

            public PressureSetting(PressureUnit PressUnit, double Value)
            {
                this.PressUnit = PressUnit;
                this.Value = Value;
            }
        }
    }
}
