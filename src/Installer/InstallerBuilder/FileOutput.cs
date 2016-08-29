using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace InstallerBuilder
{
    public static class FileOutput
    {
        private static readonly string outputFolder = "../../../Output";

        public static void Build()
        {
            ClearDirectory(outputFolder);
            var tmpFolder = Path.Combine(outputFolder, "tmp");
            Directory.CreateDirectory(tmpFolder);
            CompileApp(tmpFolder);

            var version = GetVersion(tmpFolder);
            var folder = Path.Combine(outputFolder, version);
            Directory.CreateDirectory(outputFolder);
            Directory.Move(tmpFolder, folder);

            WriteLicenseText(folder);
            CopyDirectory(Path.Combine(RepositoryRoot(), "manual"),
                Path.Combine(folder, "manual"));

            CompileLauncher();
            GenerateVersionConfig(version);

            DeleteRedundantFiles(version);
        }

        private static void DeleteRedundantFiles(string version)
        {
            var mainDir = Path.Combine(outputFolder, version);
            File.Delete(Path.Combine(mainDir, "INIFileParser.xml"));
        }

        private static void GenerateVersionConfig(string version)
        {
            var elem = new XElement("Root",
                new XElement("current", version));

            var path = Path.Combine(outputFolder, "version.xml");
            File.WriteAllText(path, new XDocument(elem).ToString());
        }
               
        private static string RepositoryRoot()
        {
            return Path.Combine(outputFolder, "../../..");
        }

        private static string ProjectFolder()
        {
            var srcPath = Path.Combine(outputFolder, "../..");
            return Path.Combine(srcPath, "QSimPlanner");
        }

        private static string ProjectFile()
        {
            return Path.Combine(ProjectFolder(), "QSimPlanner.csproj");
        }

        private static void WriteLicenseText(string folder)
        {
            var gen = new LicenseTextGenerator(RepositoryRoot());

            File.WriteAllText(Path.Combine(folder, "LICENSE.txt"),
               gen.Generate());
        }

        private static string GetVersion(string folder)
        {
            var file = Path.Combine(folder, "QSimPlan.exe");
            var ver = AssemblyName.GetAssemblyName(file).Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }

        private static void CompileApp(string folder)
        {
            Compile(ProjectFile(), folder);
        }

        private static void CompileLauncher()
        {
            var projFile = Path.Combine(RepositoryRoot(),
                "src/Launcher/Launcher.csproj");

            Compile(projFile, outputFolder);
        }

        private static void Compile(string projectFile, string outputFolder)
        {
            var properties = new ProcessStartInfo();

            properties.UseShellExecute = false;
            properties.FileName = GetMsbuildPath();
            properties.Arguments = $"{projectFile} /p:Configuration=Release " +
                $"/p:OutputPath={Path.GetFullPath(outputFolder)}";

            var process = Process.Start(properties);
            process.WaitForExit();
        }

        private static string GetMsbuildPath()
        {
            return XDocument.Load("paths.xml")
                .Root
                .Element("MsBuildExePath")
                .Value;
        }
        
        private static void CopyDirectory(string source, string target)
        {
            var info = new ProcessStartInfo();
            info.UseShellExecute = false;
            info.FileName = @"C:\WINDOWS\system32\xcopy.exe";
            info.Arguments = $"\"{source}\" \"{target}\" /E /I";
            var process = Process.Start(info);
            process.WaitForExit();
        }

        public static void ClearDirectory(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
        }
    }
}
