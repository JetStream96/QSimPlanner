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
            var version = GetVersion();
            ClearDirectory(outputFolder);
            var folder = Path.Combine(outputFolder, version);
            Directory.CreateDirectory(folder);
            WriteLicenseText(folder);
            CompileApp(folder);

            Console.WriteLine("Build completed.");
            Console.ReadKey();
        }

        private static void ClearDirectory(string folder)
        {
            if (Directory.Exists(folder))
            {
                //foreach (var i in Directory.GetFiles(folder))
                //{
                //    File.Delete(i);
                //}

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

        private static string GetVersion()
        {
            ClearDirectory(outputFolder);
            CompileApp(outputFolder);

            var file = Path.Combine(outputFolder, "QSimPlan.exe");
            var ver = AssemblyName.GetAssemblyName(file).Version;
            return $"{ver.Major}.{ver.Minor}.{ver.Build}";

            //var file = Path.Combine(
            //    ProjectFolder(), "Properties/AssemblyInfo.cs");
            //var txt = File.ReadAllText(file);
            //var ver = MatchVersion(txt);
            //return ver.Substring(0, ver.LastIndexOf('.'));
        }

        //private static string MatchVersion(string allTxt)
        //{
        //    // Match [assembly: AssemblyVersion("0.2.3.0")]
        //    var tokens = new string[]
        //    {
        //        @"^"
        //        @"\[",
        //        "assembly",
        //        ":",
        //        "AssemblyVersion",
        //        @"\(",
        //        @"""",
        //        @"([^""]+)",
        //        @"""",
        //        @"\)",
        //        @"\]"
        //    };

        //    var pattern = string.Join(@"\s*", tokens);
        //    return Regex.Match(allTxt, pattern).Groups[1].Value;
        //}

        //private static string GetVersion(string exePath)
        //{
        //    var dom = AppDomain.CreateDomain("name");
        //    var assemblyName = new AssemblyName();
        //    assemblyName.CodeBase = Path.GetFullPath(exePath);
        //    var assembly = dom.Load(assemblyName);
        //    var ver = assembly.GetName().Version;
        //    var result = $"{ver.Major}.{ver.Minor}.{ver.Build}";
        //    AppDomain.Unload(dom);
        //    return result;
        //}

        //private static void MoveToCorrectFolder(string exePath)
        //{
        //    var assemblyVersion = GetVersion(exePath);
        //    var folder = Path.GetDirectoryName(exePath);
        //    var newFolder = Path.Combine(
        //        new DirectoryInfo(folder).Parent.FullName, assemblyVersion);

        //    Directory.Move(folder, newFolder);

        //    //var files = AllFiles(folder);
        //    //var fileRelativePaths = files
        //    //    .Select(f => RelativePath(f, folder))
        //    //    .ToList();

        //    //for (int i = 0; i < files.Count; i++)
        //    //{
        //    //    var newPath = Path.Combine(newFolder, fileRelativePaths[i]);
        //    //    Directory.CreateDirectory(Path.GetDirectoryName(newPath));
        //    //    File.Move(files[i], newPath);
        //    //}
        //}

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

        private static string RelativePath(string path, string folder)
        {
            var pathUri = new Uri(Path.GetFullPath(path));

            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }

            var folderUri = new Uri(Path.GetFullPath(folder));
            var relativePath = folderUri.MakeRelativeUri(pathUri)
                .ToString()
                .Replace('/', Path.DirectorySeparatorChar);

            return Uri.UnescapeDataString(relativePath);
        }
    }
}
