using QSP.Common;
using System;
using System.IO;

namespace QSP.LibraryExtension
{
    public static class FileNameGenerator
    {
        /// <exception cref="NoFileNameAvailException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Generate(string directory,
            string nameBase, string extension)
        {
            if (directory.ContainIllegalChar() ||
                nameBase.ContainIllegalChar() ||
                extension.ContainIllegalChar())
            {
                throw new ArgumentException("Illegal chars not allowed.");
            }

            string fn = Path.Combine(directory, nameBase + extension);

            if (Directory.Exists(directory) == false ||
                File.Exists(fn) == false)
            {
                return fn;
            }

            for (int i = 0; i <= int.MaxValue; i++)
            {
                string file = Path.Combine(directory,
                    nameBase + i.ToString() + extension);

                if (File.Exists(file) == false)
                {
                    return file;
                }
            }

            throw new NoFileNameAvailException(
                "No suitable file name can be generated.");
        }
    }
}
