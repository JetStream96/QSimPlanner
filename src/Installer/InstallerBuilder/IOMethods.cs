using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace InstallerBuilder
{
    public class IOMethods
    {
        public static void ClearDirectory(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }

        public static List<string> AllFiles(string directory)
        {
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(directory));

            foreach (string j in Directory.GetDirectories(directory))
            {
                files.AddRange(AllFiles(j));
            }

            return files;
        }

        public static string RelativePath(string path, string folder)
        {
            var pathUri = new Uri(Path.GetFullPath(path));

            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }

            var folderUri = new Uri(Path.GetFullPath(folder));
            var relativePath = folderUri.MakeRelativeUri(pathUri).ToString();

            return Uri.UnescapeDataString(relativePath);
        }
    }
}
