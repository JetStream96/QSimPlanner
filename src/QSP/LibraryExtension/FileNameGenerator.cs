using QSP.Common;
using System;
using System.IO;

namespace QSP.LibraryExtension
{
    public static class FileNameGenerator
    {
        /// <exception cref="NoFileNameAvailException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Generate(
            string directory,
            string nameBase,
            Func<int, string> numberFormat,
            string extension)
        {
            if (nameBase.ContainIllegalChar() ||
                extension.ContainIllegalChar())
            {
                throw new ArgumentException(
                    "Illegal chars are not allowed.");
            }

            string fn = Path.Combine(directory, nameBase + extension);

            if (Directory.Exists(directory) == false ||
                File.Exists(fn) == false)
            {
                return fn;
            }

            int fileCount = Directory.GetFiles(directory).Length;

            for (int i = 0; i <= fileCount; i++)
            {
                string file = Path.Combine(directory,
                    nameBase + numberFormat(i) + extension);

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
