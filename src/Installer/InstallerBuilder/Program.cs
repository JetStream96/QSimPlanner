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
                    BuildAttempt();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void BuildAttempt()
        {
            var gen = new FileOutputGenerator();
            gen.Build();
            InstallerCreator.WriteFile(gen.Version);

            Console.WriteLine(@"


************************

    Build completed.

************************");
        }
    }
}
