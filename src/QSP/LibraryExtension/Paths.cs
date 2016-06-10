using System.Collections.Generic;
using System.IO;

namespace QSP.LibraryExtension
{
    public static class Paths
    {
        public static HashSet<char> IllegalChars = getIllegalChars();

        private static HashSet<char> getIllegalChars()
        {
            var result = new HashSet<char>(Path.GetInvalidFileNameChars());
            Path.GetInvalidPathChars().ForEach(c => result.Add(c));

            return result;
        }

        /// <summary>
        /// Returns whether the string contains any illegal char 
        /// (of file system).
        /// </summary>
        public static bool ContainIllegalChar(this string item)
        {
            foreach (var i in item)
            {
                if (IllegalChars.Contains(i))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Remove any illegal char (of file system).
        /// </summary>
        public static string RemoveIllegalChars(this string item)
        {
            return item.ReplaceAny(IllegalChars, "");
        }
    }
}
