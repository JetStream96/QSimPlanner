using QSP.LibraryExtension.Sets;
using System;
using System.IO;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Paths
    {
        public static readonly IReadOnlySet<char> IllegalFileNameChars =
            new ReadOnlySet<char>(Path.GetInvalidFileNameChars().ToHashSet());

        public static readonly IReadOnlySet<char> IllegalPathChars =
            new ReadOnlySet<char>(Path.GetInvalidPathChars().ToHashSet());

        /// <summary>
        /// Returns whether the string contains any illegal char (of file system).
        /// </summary>
        public static bool ContainIllegalFileNameChar(this string item)
        {
            return item.Any(c => IllegalFileNameChars.Contains(c));
        }

        public static bool ContainIllegalPathChar(this string item)
        {
            return item.Any(c => IllegalPathChars.Contains(c));
        }

        /// <summary>
        /// Remove any illegal char (of file system).
        /// </summary>
        public static string RemoveIllegalFileNameChars(this string item)
        {
            return item.ReplaceAny(IllegalFileNameChars, "");
        }

        public static string RemoveIllegalPathChars(this string item)
        {
            return item.ReplaceAny(IllegalPathChars, "");
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
