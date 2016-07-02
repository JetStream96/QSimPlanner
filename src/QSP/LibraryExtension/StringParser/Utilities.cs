using System;
using System.Linq;
using QSP.Utilities;

namespace QSP.LibraryExtension.StringParser
{
    public static class Utilities
    {
        public static readonly char[] DelimiterWords = { ' ', '\n', '\r', '\t' };

        /// <summary>
        /// Returns a substring starting from the given position to the char 
        /// before the next appearance of endChar.
        /// Finally position is moved to the char after endChar.
        /// If the char is not found, position is unchanged and null is returned.
        /// </summary>
        public static string ReadString(string item, ref int position, char endChar)
        {
            int x = item.IndexOf(endChar, position);
            return readString(item, ref position, x);
        }
        
        private static string readString(string item, ref int position, int x)
        {
            if (x < 0)
            {
                return null;
            }
            var s = item.Substring(position, x - position);
            position = x + 1;
            return s;
        }        

        /// <summary>
        /// Parse the part of string from startindex to the last char that is a digit.       
        /// </summary>
        /// <param name="endIndex">Last index which is a digit.</param>
        public static int ParseInt(string item, int startindex, out int endIndex)
        {
            ConditionChecker.Ensure<ArgumentException>(item[startindex] >= '0' &&
                                                       item[startindex] <= '9');
            int result = item[startindex] - '0';
            int negate = 1;

            for (int i = startindex + 1; i < item.Length; i++)
            {
                if (item[i] >= '0' && item[i] <= '9')
                {
                    result *= 10;
                    result += item[i] - '0';
                }
                else if (item[i] == '-' && i == startindex)
                {
                    negate = -1;
                }
                else
                {
                    endIndex = i - 1;
                    return result * negate;
                }
            }
            endIndex = item.Length - 1;
            return result * negate;
        }
        
        /// <summary>
        /// If first char of next line exists, return true and change the currentIndex 
        /// to the index of that char. Otherwise return false.
        /// </summary>
        public static bool SkipToNextLine(string item, ref int currentIndex)
        {
            int index = item.IndexOf('\n', currentIndex) + 1;

            if (index <= 0 ||                // '\n' not found
                index == item.Length)        // '\n' is the last char of string
            {
                return false;
            }

            currentIndex = index;
            return true;
        }

        /// <summary>
        /// Set index to the next index of char which is not in charsToSkip. The search starts at index.
        /// If none of the chars in charsToSkip appear, index is set to the last index of string.        
        /// </summary>
        public static void SkipAny(string item, char[] charsToSkip, ref int index)
        {
            while (index < item.Length)
            {
                if (charsToSkip.Contains(item[index]))
                {
                    index++;
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Return the substring starting from the given index, to the char before the next occurence
        /// of any char in Delimeters. If none of the chars in Delimeters appears, the returning substring
        /// contains every char after the given index.
        /// </summary>
        public static string ReadToNextDelimeter(string item, char[] Delimeters, ref int index)
        {
            int x = item.IndexOfAny(Delimeters, index);

            if (x < 0)
            {
                x = item.Length;
            }

            string str = item.Substring(index, x - index);
            index = x;
            return str;
        }

        /// <summary>
        /// Set the index to the next occurence of the given string and returns true, if found.
        /// Otherwise return false and index is unchanged.
        /// </summary>
        public static bool MoveToNextIndexOf(string item, string target, ref int index)
        {
            int x = item.IndexOf(target, index);

            if (x < 0)
            {
                return false;
            }
            index = x;
            return true;
        }

        /// <summary>
        /// Set the index to the next occurence of the given string and returns true, if found.
        /// Otherwise return false and index is unchanged.
        /// </summary>
        public static bool MoveToNextIndexOf(string item, char target, ref int index)
        {
            int x = item.IndexOf(target, index);

            if (x < 0)
            {
                return false;
            }
            index = x;
            return true;
        }
    }
}
