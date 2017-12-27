using System;
using System.IO;
using System.IO.Compression;
using static InstallerBuilder.Program;

namespace InstallerBuilder
{
    public static class AiracFile
    {
        private static string NavDataFolder => Path.Combine(OutputFolder, "../navdata");

        private static string NavDataFilePath =>
            Path.Combine(NavDataFolder, "AerosoftAirbusX_1705.zip");

        // Gets the airac file if it does not exist. Otherwise, do nothing.
        private static void GetAirac()
        {
            Directory.CreateDirectory(NavDataFolder);
            if (!File.Exists(NavDataFilePath))
            {
                // TOOD: Obtains the airac file.
                throw new NotImplementedException();
            }
        }

        private static void WriteReadmeFile()
        {
            var content = "All files in this folder is provided by Aerosoft and should not be " +
                "distributed without permission.";
            var path = Path.Combine(FileOutputGenerator.TmpOutputFolder, "NavData/readme.txt");
            File.WriteAllText(path, content);
        }

        public static void CopyNavData()
        {
            GetAirac();
            var dest = Path.Combine(FileOutputGenerator.TmpOutputFolder, "NavData");
            ZipFile.ExtractToDirectory(NavDataFilePath, dest);
            WriteReadmeFile();
        }
    }
}
