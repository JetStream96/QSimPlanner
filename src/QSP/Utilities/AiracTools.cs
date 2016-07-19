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
        public static bool AiracValid(string airacPeriod)
        {
            int monthStart = 0;
            int monthEnd = 0;
            int dayStart = 0;
            int dayEnd = 0;
            int yearStart = 0;
            int yearEnd = 0;

            try
            {
                monthStart = MonthEnglishToNum(airacPeriod.Substring(2, 3));
                monthEnd = MonthEnglishToNum(airacPeriod.Substring(7, 3));
                dayStart = Convert.ToInt32(airacPeriod.Substring(0, 2));
                dayEnd = Convert.ToInt32(airacPeriod.Substring(5, 2));
                yearStart = Convert.ToInt32(airacPeriod.Substring(11, 2));
            }
            catch
            {
                throw new ArgumentException(
                    "Input string is not valid airac period.");
            }

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
    }
}
