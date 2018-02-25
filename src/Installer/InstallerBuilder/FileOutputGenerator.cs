using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using static InstallerBuilder.IOMethods;
using static InstallerBuilder.Program;

namespace InstallerBuilder
{
    public class FileOutputGenerator
    {
        public string Version { get; private set; }

        public static string TmpOutputFolder => Path.Combine(OutputFolder, "tmp");

        public void Build()
        {
            ClearDirectory(OutputFolder);
            Directory.CreateDirectory(TmpOutputFolder);
            CompileApp(TmpOutputFolder);

            var errorReporterFolder = Path.Combine(OutputFolder, "tmperr");
            CompileErrorReport();
            File.Move(Path.Combine(errorReporterFolder, "ErrorReport.exe"),
                Path.Combine(OutputFolder, "tmp/ErrorReport.exe"));
            Directory.Delete(errorReporterFolder, true);

            AiracFile.CopyNavData();

            Version = GetVersion(TmpOutputFolder);
            var folder = Path.Combine(OutputFolder, Version);
            Directory.CreateDirectory(OutputFolder);
            Directory.Move(TmpOutputFolder, folder);

            WriteLicenseText(folder);
            CopyDirectory(Path.Combine(RepositoryRoot, "manual"),
                Path.Combine(folder, "manual"));

            CompileLauncher();
            GenerateVersionConfig(Version);

            DeleteRedundantFiles(Version);
        }
        
        private static void CompileErrorReport()
        {
            var errorReporterFolder = Path.Combine(OutputFolder, "tmperr");
            Directory.CreateDirectory("tmperr");
            var errProjectFile = Path.Combine(OutputFolder, "../../ErrorReport/ErrorReport.csproj");
            Compile(errProjectFile, errorReporterFolder);
        }

        private static string EscapeBackslash(string s) => s.Replace(@"\", @"\\");

        private static void DeleteRedundantFiles(string version)
        {
            var mainDir = Path.Combine(OutputFolder, version);
            File.Delete(Path.Combine(mainDir, "INIFileParser.xml"));
        }

        private static void GenerateVersionConfig(string version)
        {
            var elem = new XElement("Root",
                new XElement("current", version),
                new XElement("backup", ""));

            var path = Path.Combine(OutputFolder, "version.xml");
            File.WriteAllText(path, new XDocument(elem).ToString());
        }

        public static string RepositoryRoot => Path.Combine(OutputFolder, "../../..");

        private static string ProjectFolder()
        {
            var srcPath = Path.Combine(OutputFolder, "../..");
            return Path.Combine(srcPath, "QSP");
        }

        private static string ProjectFile() => Path.Combine(ProjectFolder(), "QSP.csproj");

        private static void WriteLicenseText(string folder)
        {
            var gen = new LicenseTextGenerator(RepositoryRoot);
            File.WriteAllText(Path.Combine(folder, "LICENSE.txt"), gen.Generate());
        }
        
        private static string GetVersion(string folder)
        {
            var file = Path.Combine(folder, "QSimPlanner.exe");
            var ver = AssemblyName.GetAssemblyName(file).Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";
        }

        private static void CompileApp(string folder)
        {
            Compile(ProjectFile(), folder);
        }

        private static void CompileLauncher()
        {
            var projFile = Path.Combine(RepositoryRoot, "src/Launcher/Launcher.csproj");
            Compile(projFile, OutputFolder);
        }

        private static void Compile(string projectFile, string outputFolder)
        {
            var p = Path.GetFullPath(projectFile);
            var info = new ProcessStartInfo();

            info.UseShellExecute = false;
            info.FileName = GetMsbuildPath();
            info.Arguments = $"\"{EscapeBackslash(Path.GetFullPath(projectFile))}\" " +
                "/p:Configuration=Release " + "/property:Platform=x86 " +
                $"/p:OutputPath=\"{EscapeBackslash(Path.GetFullPath(outputFolder))}\"";

            var process = Process.Start(info);
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
    }
}
