using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using static InstallerBuilder.FileOutput;

namespace InstallerBuilder
{
    public class InstallerCreator
    {
        public static void WriteFile(string version, string outputFolder)
        {
            CreateResultsFolder(outputFolder);

        }

        private static string GetFileText(string version)
        {
            var text = File.ReadAllText("template.iss");
            text = text.Replace("AppVersion=",
                $"AppVersion={version}");

        }

        private static void CreateResultsFolder(string outputFolder)
        {
            var path = Path.Combine(outputFolder, "../Results");
            ClearDirectory(path);
            Directory.CreateDirectory(path);
        }
    }
}
