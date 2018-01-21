using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace InstallerBuilder
{
    public static class NavDataFile
    {
        private static readonly string Url = 
            "https://github.com/JetStream96/QSimPlanner/releases/download/v0.4.1/QSimPlanner_0.4.1_portable.zip";

        // MD5 hash of the above file.
        private static readonly string md5Hash = "15765660c39301ccbbad7a2d7b9af259";

        private static readonly string FilePath = "../../../navdata/AerosoftAirbusX_1705.zip";


        /// <summary>
        /// Checks if the file is already in place. If not, downloads the file.
        /// </summary>
        public static void PrepareFile()
        {

        }

        public static string GetMD5Hash()
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(@"C:\Users\David\Downloads\QSimPlanner_0.4.1_portable.zip"))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
