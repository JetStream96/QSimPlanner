using System.IO;

namespace QSP.LibraryExtension
{
    public static class IOMethods
    {
        public static void ClearDirectory(string folder)
        {
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
        }

        public static void CopyDirectory(string source, string target, bool overwrite = false)
        {
            CopyDirectory(new DirectoryInfo(source), new DirectoryInfo(target), overwrite);
        }

        public static void CopyDirectory(DirectoryInfo source,
            DirectoryInfo target, bool overwrite = false)
        {
            foreach (var dir in source.GetDirectories())
            {
                CopyDirectory(dir, target.CreateSubdirectory(dir.Name), overwrite);
            }

            foreach (var file in source.GetFiles())
            {
                file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
            }
        }
    }
}
