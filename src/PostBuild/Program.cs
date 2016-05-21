using System.IO;

namespace PostBuild
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText("LICENSE.txt", LicenseTextGenerator.Generate());            
        }
    }
}
