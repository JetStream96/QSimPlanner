using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace PostBuilt
{
    public static class LicenseTextGenerator
    {
        private static readonly string license = @"../../../../LICENSE.txt";
        private static readonly string thirdParty = @"../../../../third-party";

        private static readonly string target = "******************";

        public static string Generate()
        {
            var output = new StringBuilder(modifiedMainLicense());
            var files = Directory.GetFiles(thirdParty);

            foreach (var i in files)
            {
                var fileName = Path.GetFileName(i);
                var package = Regex.Replace(
                    fileName, @".txt", "", RegexOptions.IgnoreCase);

                output.Append(package);
                output.Append("\n\n");
                output.Append(File.ReadAllText(i));
                output.Append("\n\n" + new string('-', 75) + "\n\n");
            }

            return output.ToString();
        }

        private static string modifiedMainLicense()
        {
            var output = new StringBuilder();
            var mainTxt = File.ReadAllText(license);
            int index = mainTxt.IndexOf(target);

            if (index == -1)
            {
                throw new ArgumentException("Wrong LICENSE.txt format.");
            }

            output.Append(mainTxt.Substring(0, index));
            output.Append(
                @"*******************************************************************************

This application uses third-party packages. Their licenses are included below:

*******************************************************************************");
            output.Append("\n\n");
            return output.ToString();
        }
    }
}
