using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PostBuilt
{
    class Program
    {
        static void Main(string[] args)
        {
            File.WriteAllText("LICENSE.txt", LicenseTextGenerator.Generate());            
        }
    }
}
