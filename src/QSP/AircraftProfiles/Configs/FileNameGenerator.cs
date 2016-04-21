using System.IO;

namespace QSP.AircraftProfiles.Configs
{
    public static class FileNameGenerator
    {
        /// <exception cref="NoFileNameAvailException"></exception>
        public static string Generate(string directory,
            string ac, string registration)
        {
            string fileNameBase = ac + "_" + registration;
            string fn = Path.Combine(directory, fileNameBase + ".ini");

            if (Directory.Exists(directory) == false ||
                File.Exists(fn) == false)
            {
                return fn;
            }

            for (int i = 0; i <= int.MaxValue; i++)
            {
                string file = Path.Combine(directory,
                    fileNameBase + i.ToString() + ".ini");

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
