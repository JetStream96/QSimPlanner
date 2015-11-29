using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace QSP.LibraryExtension
{

    public static class LibraryExtension
    {
        public static void Swap<T>(ref T d1, ref T d2)
        {
            var d = d2;
            d2 = d1;
            d1 = d;
        }
        
        /// <summary>
        /// Returns a new string where all occurence in oldValue is replaced by newValue.
        /// </summary>
        public static string ReplaceString(string input, string[] oldValue, string newValue)
        {
            string result = input;

            foreach (var elem in oldValue)
            {
                result = result.Replace(elem, newValue);
            }

            return result;
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
                    throw new ArgumentException("Input string is not a valid month identifier.");
            }
        }

        /// <summary>
        /// Returns a substring starting with the given index. Total number of elements examined is given by "length". 
        /// The returning string may be shorter than "length" since any item in ignoredItems is NOT added to resulting string.
        /// </summary>
        public static string Substring(this string item, int index, int length, char[] ignoredItems)
        {
            int count = 0;
            char[] result = new char[length];

            for (int i = index; i <= index + length - 1; i++)
            {
                if (!ignoredItems.Contains(item[i]))
                {
                    result[count] = item[i];
                    count++;
                }
            }
            return new string(result, 0, count);
        }
                
        public static bool ToBool(this XElement x)
        {
            return x.Value.ToBool();
        }

        public static bool ToBool(this string s)
        {
            if (s == "True")
            {
                return true;
            }
            else if (s == "False")
            {
                return false;
            }
            else
            {
                throw new ArgumentException("Invalid string to represent boolean.");
            }
        }

    }

}


