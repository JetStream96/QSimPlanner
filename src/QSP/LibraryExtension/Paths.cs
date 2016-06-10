using System.IO;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Paths
    {
        public static char[] IllegalChars = getIllegalChars();

        private static char[] getIllegalChars()
        {
            var invalid = Path.GetInvalidFileNameChars().ToList();
            invalid.AddRange(Path.GetInvalidPathChars());
            return invalid.ToArray();
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
