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
        static void Main(string[] args)
        {
            try
            {
                FileOutput.Build();

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
