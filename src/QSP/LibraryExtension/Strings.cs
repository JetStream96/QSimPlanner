using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QSP.LibraryExtension
{
    public static class Strings
    {
        public static string ShiftToRight(this string str, int steps)
        {
            return new string(' ', steps) +
                str.Replace("\n", "\n" + new string(' ', steps));
        }

        public static string ReplaceAny(this string input,
            IEnumerable<char> oldValue, string newValue)
        {
            var sb = new StringBuilder(input.Length);

            foreach (var c in input)
            {
                if (oldValue.Contains(c))
                {
                    sb.Append(newValue);
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        public static string RemoveHtmlTags(this string item)
        {
            var array = new char[item.Length];
            bool copyChar = true;
            int index = 0;

            foreach (var i in item)
            {
                if (i == '<')
                {
                    copyChar = false;
                }
                else if (i == '>')
                {
                    copyChar = true;
                }
                else
                {
                    if (copyChar)
                    {
                        array[index++] = i;
                    }
                }
            }
            return new string(array, 0, index);
        }

        public static string[] Lines(this string item)
        {
            return item.Split(
                new string[] { "\r\n", "\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Trim all leading and trailing empty lines of a string.
        /// </summary>
        public static string TrimEmptyLines(this string input)
        {
            var lines = input.Lines();
            var skipFront = lines.SkipEmptyLines();
            var skipBack = skipFront.Reverse().SkipEmptyLines().Reverse();
            return string.Join("\n", skipBack);
        }

        private static IEnumerable<string>
            SkipEmptyLines(this IEnumerable<string> item)
        {
            char[] spaces = { ' ', '\t' };
            return item.SkipWhile(s => s.All(c => spaces.Contains(c)));
        }

        public static bool EqualsIgnoreNewlineStyle(
            this string item, string other)
        {
            item = item.Replace("\r\n", "\n");
            other = other.Replace("\r\n", "\n");
            return item == other;
        }
    }
}

