using System;

namespace QSP.LibraryExtension
{
    public static class Utilities
    {
        public static void Swap<T>(ref T item1, ref T item2)
        {
            var tmp = item2;
            item2 = item1;
            item1 = tmp;
        }

        public static int MonthEnglishToNum(string month)
        {
            switch (month)
            {
                case "JAN":
                    return 1;
                case "FEB":
                    return 2;
                case "MAR":
                    return 3;
                case "APR":
                    return 4;
                case "MAY":
                    return 5;
                case "JUN":
                    return 6;
                case "JUL":
                    return 7;
                case "AUG":
                    return 8;
                case "SEP":
                    return 9;
                case "OCT":
                    return 10;
                case "NOV":
                    return 11;
                case "DEC":
                    return 12;
                default:
                    throw new ArgumentException(
                        "Input string is not a valid month identifier.");
            }
        }
    }

}


