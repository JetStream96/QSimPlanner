using QSP.Common;
using System;
using System.IO;

namespace QSP.LibraryExtension
{
    public static class FileNameGenerator
    {
        /// <summary>
        /// Find a name for the file such that no file with the same
        /// name exists.
        /// </summary>
        /// <exception cref="NoFileNameAvailException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static string Generate(
            string directory,
            string nameBase,
            string extension,
            Func<int, string> numberFormat,
            int startNumber = 1)
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
            
            for (int i = startNumber; ; i++)
            {
                string file = Path.Combine(directory,
                    nameBase + numberFormat(i) + extension);

                if (File.Exists(file) == false) return file;
            }

            throw new NoFileNameAvailException(
                "No suitable file name can be generated.");
        }
    }
}
