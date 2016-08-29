using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static InstallerBuilder.IOMethods;
using static InstallerBuilder.Program;

namespace InstallerBuilder
{
    public static class InstallerCreator
    {
        public static void WriteFile(string version)
        {
            CreateResultsFolder();

            var text = GetFileText(version);
            var filePath = Path.Combine(outputFolder, "build.iss");
            File.WriteAllText(filePath, text);

            BuildInstaller(filePath);
        }

        private static void BuildInstaller(string issPath)
        {
            var result = Path.GetFullPath(ResultsFolderPath());
            var iss = Path.GetFullPath(issPath);

            var info = new ProcessStartInfo();

            info.UseShellExecute = false;
            info.WorkingDirectory = GetInnoSetupPath();
            info.FileName = Path.Combine(GetInnoSetupPath(), "iscc.exe");
            info.Arguments = $"/O\"{result}\" \"{iss}\"";

            var process = Process.Start(info);
            process.WaitForExit();
        }

        private static string GetInnoSetupPath()
        {
            return XDocument.Load("paths.xml")
                .Root
                .Element("InnoSetupDirectory")
                .Value;
        }

        private static string GetFileText(string version)
        {
            var text = File.ReadAllText("template.iss");
            text = text.Replace("AppVersion=",
                $"AppVersion={version}");
            return text.Replace("[Files]",
                "[Files]\n" + FileList());
        }

        private static string FileList()
        {
            var files = AllFiles(outputFolder)
                .Select(Path.GetFullPath)
                .Select(p => Tuple.Create(p, RelativePath(p, outputFolder)));

            var lines = files
                 .Select(f =>
                 $"Source: \"{f.Item1}\"; " +
                 $"DestDir: \"{{app}}\\{Path.GetDirectoryName(f.Item2)}\"");

            return string.Join("\n", lines);
        }

        private static string ResultsFolderPath()
        {
            return Path.Combine(outputFolder, "../Results");
        }

        private static void CreateResultsFolder()
        {
            var path = ResultsFolderPath();
            ClearDirectory(path);
            Directory.CreateDirectory(path);
        }
    }
}
