using System.IO;

namespace QSP.LibraryExtension
{
    public static class IOMethods
    {
        public static void ClearDirectory(string folder)
        {
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
        }

        public static void CopyDirectory(string source, string target)
        {
            CopyDirectory(new DirectoryInfo(source),
                new DirectoryInfo(target));
        }

        public static void CopyDirectory(DirectoryInfo source, 
            DirectoryInfo target)
        {
            foreach (var dir in source.GetDirectories())
                CopyDirectory(dir, target.CreateSubdirectory(dir.Name));

            foreach (var file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name));
        }
    }
}
