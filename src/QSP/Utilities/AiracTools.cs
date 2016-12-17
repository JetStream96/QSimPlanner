using System;
using System.IO;
using System.Linq;
using static QSP.LibraryExtension.Utilities;

namespace QSP.Utilities
{
    public static class AiracTools
    {
        /// <summary>
        /// Reads from file and gets the AIRAC cycle and valid period. 
        /// e.g. { Cycle: "1407", Period: "26JUN23JUL/14" }.
        /// </summary>
        /// <param name="folderpath">The folder containing cycle.txt.</param>
        public static AiracPeriod AiracCyclePeriod(string folderpath)
        {
            var fileLocation = Path.Combine(folderpath, "cycle.txt");
            string str = File.ReadAllText(fileLocation);
            var s = str.Split(',').Select(i => i.Trim()).ToList();

            return new AiracPeriod(s[0], s[1], ParsePeriod(s[1]));
        }

        public static AiracPeriod TryGetAiracCyclePeriod(string folderPath)
        {
            try
            {
                return AiracCyclePeriod(folderPath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Parse the AIRAC cycle period.
        /// </summary>
        /// <param name="airacPeriod">For example, 26JUN23JUL/14.</param>
        /// <exception cref="ArgumentException"></exception>
        public static TimePeriod ParsePeriod(string airacPeriod)
        {
            try
            {
                var a = airacPeriod;
                int monthStart = MonthEnglishToNum(a.Substring(2, 3));
                int monthEnd = MonthEnglishToNum(a.Substring(7, 3));
                int dayStart = int.Parse(a.Substring(0, 2));
                int dayEnd = int.Parse(a.Substring(5, 2));
                int yearStart = int.Parse(a.Substring(11, 2));
                int yearEnd = monthEnd < monthStart ? yearStart + 1 : yearStart;

                var dateStart = new DateTime(2000 + yearStart, monthStart, dayStart);
                var dateEnd = new DateTime(2000 + yearEnd, monthEnd, dayEnd);

                return new TimePeriod(dateStart, dateEnd);
            }
            catch
            {
                throw new ArgumentException("Input string is not valid airac period.");
            }
        }
    }
}

