using System;

namespace InstallerBuilder
{
    class Program
    {
        public static readonly string OutputFolder = "../../../Output";

        static void Main(string[] args)
        {
            const int numTry = 3;
            var asterisks = new string('*', 24);

            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine($"\n\n\n{asterisks}\n\n    " +
                    $"Build attempt {i}/{numTry}\n\n{asterisks}\n");

                try
                {
                    var doNotCreateInstaller = args.Length >= 1 && args[0] == "--no-installer";
                    BuildAttempt(!doNotCreateInstaller);
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void BuildAttempt(bool createInstaller)
        {
            var gen = new FileOutputGenerator();
            gen.Build();

            if (createInstaller) InstallerCreator.WriteFile(gen.Version);

            Console.WriteLine(@"


************************

    Build completed.

************************");
        }
    }
}
