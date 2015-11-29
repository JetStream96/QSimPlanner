using System.Collections.Generic;

namespace QSP.LibraryExtension
{

    public static class StringUtilities
    {

        public static string ShiftStringToRight(string str, int steps)
        {
            return new string(' ', steps) + str.Replace("\n", "\n" + new string(' ', steps));
        }

        public enum CutStringOptions
        {

            PreserveBoth,
            PreserveStart,
            PreserveEnd,
            PreserveNone

        }

        /// <summary>
        /// Returns the first occurence of string starting with startStr and ending with endStr.
        /// </summary>
        public static string StringStartEndWith(string original, string startStr, string endStr, CutStringOptions options)
        {

            int x = original.IndexOf(startStr);
            int y = original.IndexOf(endStr, x);
            string s = original.Substring(x, y - x + endStr.Length);

            switch (options)
            {
                case CutStringOptions.PreserveStart:
                    s = s.Replace(endStr, "");
                    break;
                case CutStringOptions.PreserveEnd:
                    s = s.Replace(startStr, "");
                    break;
                case CutStringOptions.PreserveNone:
                    s = s.Replace(endStr, "");
                    s = s.Replace(startStr, "");
                    break;
            }

            return s;

        }

        /// <summary>
        ///   Returns the substring such that:
        ///     (1) the first line contains startStr 
        ///     (2) ends with endStr
        /// </summary>
        public static string CutString2(string original, string startStr, string endStr, bool preserveEnd)
        {

            int x = original.IndexOf(startStr);
            int y = original.IndexOf(endStr, x);

            if (x == -1 | y == -1)
            {
                return null;
            }


            while (x > 0)
            {
                x --;
                if (original[x] == '\n')
                {
                    x ++;
                    break; 
                }

            }

            string s = null;

            if (preserveEnd == false)
            {
                s = original.Substring(x, y - x);
            }
            else
            {
                s = original.Substring(x, y - x + endStr.Length);
            }

            return s;

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
            int startIndex = -STR_LENGTH;


            while (startIndex != -1)
            {
                startIndex += STR_LENGTH;
                startIndex = input.IndexOf(target, startIndex);
                count ++;

                if (count == n)
                {
                    return startIndex;
                }

            }

            return -1;

        }
        
        public static string StringBetween(this string item, int index1, int index2)
        {
            return item.Substring(index1 + 1, index2 - index1 - 1);
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

                index ++;

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
            if (item==null)
            {
                item = string.Empty;
            }
        }

    }

}

