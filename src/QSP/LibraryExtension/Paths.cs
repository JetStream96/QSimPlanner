using System.IO;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Paths
    {
        public static string RemoveIllegalChars(this string item)
        {
            //string invalid = new string(Path.GetInvalidFileNameChars()) +
            //    new string(Path.GetInvalidPathChars());

            var invalid = Path.GetInvalidFileNameChars().ToList();
            invalid.AddRange(Path.GetInvalidPathChars());

            return item.ReplaceAny(invalid.ToArray(), "");            
        }
    }
}
