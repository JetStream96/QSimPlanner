using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace A319Fuel
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        // Convert to Boeing format.
        static void Convert()
        {
            var doc = XDocument.Load("./profile/A319_100_CFM.xml");

        }
    }
}
