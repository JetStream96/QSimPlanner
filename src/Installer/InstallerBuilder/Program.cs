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

namespace InstallerBuilder
{
    class Program
    {
        private static readonly string outputFolder = "../../../Output";//"Output";

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

            Console.WriteLine("Build completed.");
            Console.ReadKey();
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
            var properties = new ProcessStartInfo();

            properties.UseShellExecute = false;
            properties.FileName =
                @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe";
            var projPath = ProjectFile();

            properties.Arguments = $"{projPath} /p:Configuration=Release " +
                $"/p:OutputPath={Path.GetFullPath(folder)}";

            var process = Process.Start(properties);
            process.WaitForExit();
        }

        private static List<string> AllFiles(string directory)
        {
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(directory));

            foreach (string j in Directory.GetDirectories(directory))
            {
                files.AddRange(AllFiles(j));
            }

            return files;
        }        
    }
}
