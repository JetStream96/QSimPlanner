using System;
using System.IO;
using System.IO.Compression;

namespace InstallerBuilder
{
    class Program
    {
        public static readonly string OutputFolder = "../../../../Output";

        static void Main(string[] args)
        {
            const int numTry = 3;
            var asterisks = new string('*', 24);

            for (int i = 1; i <= numTry; i++)
            {
                Console.WriteLine($"\n\n\n{asterisks}\n\n    " +
                    $"Build attempt {i}/{numTry}\n\n{asterisks}\n");

                try
                {
                    var createAll = args.Length >= 1 && args[0] == "--all";
                    BuildAttempt(createAll);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void BuildAttempt(bool createAll)
        {
            var gen = new FileOutputGenerator();
            gen.Build();

            if (createAll)
            {
                InstallerCreator.WriteFile(gen.Version);
                PrepareReleaseFiles(gen.Version);
            }

            Console.WriteLine(@"


************************

    Build completed.

************************");
        }

        private static void PrepareReleaseFiles(string ver)
        {
            var portable = Path.Combine(InstallerCreator.ResultsFolderPath(),
                $"QSimPlanner_{ver}_portable.zip");
            ZipFile.CreateFromDirectory(OutputFolder, portable);
            var update = Path.Combine(InstallerCreator.ResultsFolderPath(),
                $"{ver}.zip");
            ZipFile.CreateFromDirectory(Path.Combine(OutputFolder, ver), update);
        }
    }
}
