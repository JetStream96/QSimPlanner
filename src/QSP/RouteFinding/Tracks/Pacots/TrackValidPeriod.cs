using System.Text.RegularExpressions;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public static class TrackValidPeriod
    {
        /// <summary>
        /// Use the given string to to find StartTime and EndTime. 
        /// The given string must contain 2 substrings of form "11061200UTC". 
        /// Otherwise both StartTime and EndTime will be empty strings.
        /// </summary>
        public static ValidPeriod GetValidPeriod(string item)
        {
            // E.g. Try to match times in
            // "BETWEEN 02161200UTC AND 02161600UTC,"

            var pattern = @"\b\d{8}UTC\b";
            var matches = Regex.Matches(item, pattern, RegexOptions.Multiline);

            if (matches.Count < 2)
            {
                return new ValidPeriod() { Start = "", End = "" };
            }

            return new ValidPeriod()
            {
                Start = matches[0].Value,
                End = matches[1].Value
            };
        }

        public struct ValidPeriod { public string Start, End; }
    }
}
