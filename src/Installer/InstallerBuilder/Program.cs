using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace InstallerBuilder
{
    class Program
    {
        private static readonly string outputFolder = "../../../Output";

        static void Main(string[] args)
        {
            try
            {
                Build();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

#if DEBUG
            Console.ReadKey();
#endif
        }

        private static void Build()
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

            Console.WriteLine("Build completed.");
        }

        private static void GenerateVersionConfig(string version)
        {
            var elem = new XElement("Root",
                new XElement("current", version));

            var path = Path.Combine(outputFolder, "version.xml");
            File.WriteAllText(path, new XDocument(elem).ToString());
        }

        private static void ClearDirectory(string folder)
        {
            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }
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
            properties.FileName =
                @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";

            properties.Arguments = $"{projectFile} /p:Configuration=Release " +
                $"/p:OutputPath={Path.GetFullPath(outputFolder)}";

            var process = Process.Start(properties);
            process.WaitForExit();
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
