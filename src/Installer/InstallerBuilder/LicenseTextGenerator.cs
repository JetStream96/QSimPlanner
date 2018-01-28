using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InstallerBuilder
{
    public class LicenseTextGenerator
    {
        private string license, thirdParty;

        public LicenseTextGenerator(string rootFolder)
        {
            license = Path.Combine(rootFolder, "LICENSE.txt");
            thirdParty = Path.Combine(rootFolder, "third-party");
        }

        public string Generate()
        {
            var output = new StringBuilder(File.ReadAllText(license));
            var files = IOMethods.AllFiles(thirdParty);

            foreach (var i in files)
            {
                var fileName = Path.GetFileName(i);
                var package = Regex.Replace(fileName, @".txt", "", RegexOptions.IgnoreCase);

                output.Append(package);
                output.Append("\n\n");
                output.Append(File.ReadAllText(i));
                output.Append("\n\n" + new string('-', 75) + "\n\n");
            }

            return output.ToString();
        }
    }
}
