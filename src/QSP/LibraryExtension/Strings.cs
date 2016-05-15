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

        public static int NthOccurence(string input, string target, int n)
        {
            int count = 0;
            int len = target.Length;
            int index = 0;

            while (true)
            {
                index = input.IndexOf(target, index);

                if (index < 0)
                {
                    return -1;
                }
                count++;

                if (count == n)
                {
                    return index;
                }
                index += len;
            }
        }

        public static List<int> IndicesOf(this string item,
            string target, int index, int count)
        {
            var result = new List<int>();
            int len = index + count - 1;

            while (index < len)
            {
                index = item.IndexOf(target, index);

                if (index < 0)
                {
                    return result;
                }
                else
                {
                    result.Add(index);
                }
                index++;
            }
            return result;
        }

        public static List<int> IndicesOf(this string item, string target)
        {
            return item.IndicesOf(target, 0, item.Length);
        }

        public static void SetEmptyIfNull(ref string item)
        {
            if (item == null)
            {
                item = string.Empty;
            }
        }

        /// <summary>
        /// Returns a new string where all occurence in oldValue 
        /// is replaced by newValue.
        /// </summary>
        public static string ReplaceAny(this string input,
            string[] oldValue, string newValue)
        {
            string result = input;

            foreach (var elem in oldValue)
            {
                result = result.Replace(elem, newValue);
            }

            return result;
        }

        public static string ReplaceAny(this string input,
            char[] oldValue, string newValue)
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
    }
}

