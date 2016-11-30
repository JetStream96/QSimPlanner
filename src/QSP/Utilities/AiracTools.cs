using System;
using static QSP.LibraryExtension.Utilities;

namespace QSP.Utilities
{
    public static class AiracTools
    {
        /// <summary>
        /// Determines whether the AIRAC cycle is within valid period.
        /// </summary>
        /// <param name="airacPeriod">For example, 26JUN23JUL/14.</param>
        /// <exception cref="ArgumentException"></exception>
        public static bool AiracValid(string airacPeriod)
        {
            try
            {
                var a = airacPeriod;
                int monthStart = MonthEnglishToNum(a.Substring(2, 3));
                int monthEnd = MonthEnglishToNum(a.Substring(7, 3));
                int dayStart = Convert.ToInt32(a.Substring(0, 2));
                int dayEnd = Convert.ToInt32(a.Substring(5, 2));
                int yearStart = Convert.ToInt32(a.Substring(11, 2));
                int yearEnd;

                if (monthEnd < monthStart)
                {
                    yearEnd = yearStart + 1;
                }
                else
                {
                    yearEnd = yearStart;
                }

                var dateStart =
                    new DateTime(2000 + yearStart, monthStart, dayStart);

                var dateEnd = new DateTime(2000 + yearEnd, monthEnd, dayEnd);
                var dateNow = DateTime.UtcNow.Date;

                return (dateStart <= dateNow && dateNow <= dateEnd);
            }
            catch
            {
                throw new ArgumentException(
                   "Input string is not valid airac period.");
            }
        }
    }
}
