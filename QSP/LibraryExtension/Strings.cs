using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QSP.LibraryExtension
{
    public static class Strings
    {
        public static string ShiftStringToRight(string str, int steps)
        {
            return new string(' ', steps) + str.Replace("\n", "\n" + new string(' ', steps));
        }
        
        public static string CenterString(string item, int totalLength)
        {
            int len = item.Length;

            if (len <= totalLength)
            {
                return item;
            }
            else
            {
                string result = null;
                int left = (totalLength - len) / 2;

                result = item.PadLeft(left, ' ');
                result = result.PadRight(totalLength - len - left, ' ');

                return result;
            }
        }

        public static int NthOccurence(string input, string target, int n)
        {
            int count = 0;
            int STR_LENGTH = target.Length;
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
                index += STR_LENGTH;
            }
        }

        public static List<int> IndicesOf(this string item, string target, int index, int count)
        {
            List<int> result = new List<int>();
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

        public static List<int> IndicesOf(this string item, string target, int index)
        {
            return item.IndicesOf(target, index, item.Length - index);
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
        /// Returns a new string where all occurence in oldValue is replaced by newValue.
        /// </summary>
        public static string ReplaceAny(this string input, string[] oldValue, string newValue)
        {
            string result = input;

            foreach (var elem in oldValue)
            {
                result = result.Replace(elem, newValue);
            }

            return result;
        }

        public static string ReplaceAny(this string input, char[] oldValue, string newValue)
        {
            var sb = new StringBuilder(input.Length);
            int index = 0;

            while (index < input.Length)
            {
                int tmp = input.IndexOfAny(oldValue, index);

                if (tmp >= 0)
                {
                    sb.Append(input, index, tmp - index);
                    sb.Append(newValue);
                    index = tmp + 1;
                }
                else
                {
                    sb.Append(input, index, input.Length - index);
                    break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a substring starting with the given index. 
        /// Total number of elements examined is given by "length". 
        /// The returning string may be shorter than "length" since 
        /// any item in ignoredItems is NOT added to resulting string.
        /// </summary>
        public static string Substring(this string item, int index, int length, char[] ignoredItems)
        {
            int count = 0;
            char[] result = new char[length];

            for (int i = index; i < index + length; i++)
            {
                if (!ignoredItems.Contains(item[i]))
                {
                    result[count] = item[i];
                    count++;
                }
            }
            return new string(result, 0, count);
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
    }
}

