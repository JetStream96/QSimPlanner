using QSP.LibraryExtension;
using static QSP.LibraryExtension.StringUtilities;

namespace QSP.RouteFinding.Tracks.Pacots
{
    public static class TrackValidPeriod
    {
        private static char[] delimiters = { ' ', '\r', '\n', '\t' };

        /// <summary>
        /// Use the given string to to find StartTime and EndTime. 
        /// The given string must contain 2 substrings of form "11061200UTC". 
        /// Otherwise both StartTime and EndTime will be empty strings.
        /// </summary>
        public static Pair<string, string> GetValidPeriod(string item)
        {
            int index = 0;
            var start = findTimeStamp(item, ref index);
            string end = null;

            if (start != null)
            {
                end = findTimeStamp(item, ref index);

                // If end cannot be found, set both start and end to null. 
                if (end == null)
                {
                    start = null;
                }
            }

            // Convert any null to empty string.
            SetEmptyIfNull(ref start);
            SetEmptyIfNull(ref end);
            return new Pair<string, string>(start, end);
        }

        private static string findTimeStamp(string item, ref int index)
        {
            //returning string format: 11061200UTC
            int currentIndex = index;
            int matchCount = 0;
            int ALL_MATCH_LEN = 11;

            while (currentIndex >= 0 && currentIndex < item.Length)
            {
                if (currentCharMatch(matchCount, item[currentIndex]))
                {

                    if (matchCount == ALL_MATCH_LEN - 1)
                    {
                        index = currentIndex + 1;
                        return item.Substring(currentIndex - ALL_MATCH_LEN + 1, ALL_MATCH_LEN);
                    }
                    else
                    {
                        currentIndex++;
                        matchCount++;
                    }

                }
                else
                {
                    //char does not match
                    if (currentIndex + 1 < item.Length)
                    {
                        currentIndex = item.IndexOfAny(delimiters, currentIndex + 1) + 1;
                        matchCount = 0;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        private static bool currentCharMatch(int matchCount, char currentChar)
        {
            if (matchCount < 8)
            {
                if (currentChar >= '0' && currentChar <= '9')
                {
                    return true;
                }
                return false;
            }
            else if (matchCount == 8 && currentChar == 'U')
            {
                return true;
            }
            else if (matchCount == 9 && currentChar == 'T')
            {
                return true;
            }
            else if (matchCount == 10 && currentChar == 'C')
            {
                return true;
            }
            return false;
        }

    }
}
