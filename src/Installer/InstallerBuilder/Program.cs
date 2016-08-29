using System;

namespace InstallerBuilder
{
    class Program
    {
        public static readonly string outputFolder = "../../../Output";
        
        static void Main(string[] args)
        {
            try
            {
                var gen = new FileOutputGenerator();
                gen.Build();
                InstallerCreator.WriteFile(gen.Version);

                Console.WriteLine("Build completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

#if DEBUG
            Console.ReadKey();
#endif
        }
    }
}
