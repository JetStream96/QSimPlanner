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

        /// <summary>
        /// Note: The consequence of this method means NavData is put into the 
        /// same directory as QSimPlanner.exe. This is not where it's supposed 
        /// to be, but it has to stay like this for compatibility with older 
        /// version. When application starts, the NavData directory will be 
        /// moved one level up. 
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void CopyNavData()
        {
            var dest = Path.Combine(FileOutputGenerator.TmpOutputFolder, "NavData");
            ZipFile.ExtractToDirectory(NavDataFilePath, dest);
        }
    }
}
