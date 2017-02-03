using System;
using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class Dates
    {
        private static readonly IReadOnlyDictionary<string, int> monthLookup =
            new Dictionary<string, int>()
            {
                ["JAN"] = 1,
                ["FEB"] = 2,
                ["MAR"] = 3,
                ["APR"] = 4,
                ["MAY"] = 5,
                ["JUN"] = 6,
                ["JUL"] = 7,
                ["AUG"] = 8,
                ["SEP"] = 9,
                ["OCT"] = 10,
                ["NOV"] = 11,
                ["DEC"] = 12
            };

        public static int MonthEnglishToNum(string month)
        {
            try
            {
                return monthLookup[month];
            }
            catch (Exception)
            {
                throw new ArgumentException("Input string is not a valid month identifier.");
            }
        }
    }
}
