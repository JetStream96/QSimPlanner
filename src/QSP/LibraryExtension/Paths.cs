using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace QSP.LibraryExtension
{
    public static class Paths
    {
        public static HashSet<char> IllegalChars = GetIllegalChars();

        private static HashSet<char> GetIllegalChars()
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
            return item.Any(c => IllegalChars.Contains(c));
        }

        /// <summary>
        /// Remove any illegal char (of file system).
        /// </summary>
        public static string RemoveIllegalChars(this string item)
        {
            return item.ReplaceAny(IllegalChars, "");
        }
        /// <summary>
        /// Get uri from an absolute or relative path.
        /// </summary>
        public static Uri GetUri(string path)
        {
            return new Uri(Path.GetFullPath(path));
        }

        public static bool PathsAreSame(string path1, string path2)
        {
           return string.Compare(
                Path.GetFullPath(path1).TrimEnd('\\'),
                Path.GetFullPath(path2).TrimEnd('\\'),
                StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        
    }
}
