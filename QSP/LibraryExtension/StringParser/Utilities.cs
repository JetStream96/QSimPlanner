using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.StringParser
{
    public static class Utilities
    {
        /// <summary>
        /// Returns a substring starting from the given position. 
        /// The last char of this substring is the one before the next appearance of endChar.
        /// Finally position is moved to the char after endChar.
        /// </summary>
        public static string ReadString(string item, ref int position, char endChar)
        {
            int x = item.IndexOf(endChar, position);
            var s = item.Substring(position, x - position);
            position = x + 1;
            return s;
        }

        /// <summary>
        /// Parse the part of string between startindex and endIndex (inclusive).
        /// </summary>
        public static int ParseInt(string item, int startindex, int endIndex)
        {
            int result = 0;
            short negate = 1;

            for (int i = startindex; i <= endIndex; i++)
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
                    throw new ArgumentException();
                }
            }
            return result * negate;
        }

        /// <summary>
        /// Parse the part of string starting from the given position, 
        /// and ends at the char before the next appearance of endChar (or the last char of string).
        /// Finally position is moved to the char after endChar.
        /// </summary>
        public static int ParseInt(string item, ref int position, char endChar)
        {
            int result = 0;
            short negate = 1;

            for (int i = position; i < item.Length; i++)
            {
                if (item[i] >= '0' && item[i] <= '9')
                {
                    result *= 10;
                    result += item[i] - '0';
                }
                else if (item[i] == '-' && i == position)
                {
                    negate = -1;
                }
                else
                {
                    if (endChar == item[i])
                    {
                        position = i + 1;
                        return result * negate;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
            position = item.Length;
            return result * negate;
        }

        /// <summary>
        /// Parse the part of string starting from the given position, 
        /// and ends at the char before the next appearance of endChar.
        /// Finally position is moved to the char after endChar.
        /// </summary>
        public static double ParseDouble(string item, ref int position, char endChar)
        {
            int endIndex = item.IndexOf(endChar, position + 1);
            double result = ParseDouble(item, position, endIndex - 1);
            position = endIndex + 1;
            return result;
        }

        /// <summary>
        /// Parse the part of string between startindex and endIndex (inclusive).
        /// </summary>
        public static double ParseDouble(string item, int startindex, int endIndex)
        {
            double result = 0.0;
            short negate = 1;

            for (int i = startindex; i <= endIndex; i++)
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
                else if (item[i] == '.')
                {
                    double dec = 0.0;

                    for (int j = endIndex; j > i; j--)
                    {
                        if (item[j] > '9' || item[j] < '0')
                        {
                            throw new ArgumentException();
                        }
                        dec *= 0.1;
                        dec += item[j] - '0';
                    }
                    result += 0.1 * dec;
                    break;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            return result * negate;
        }
    }
}
