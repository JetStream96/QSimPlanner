using QSP.LibraryExtension;
using QSP.Utilities;
using System;
using System.IO;
using static QSP.LibraryExtension.WebRequests;

namespace ErrorReport
{
    class Program
    {
        // args: [url, data]
        // Returns 1 if reporting failed, or the response from server is not "OK".
        static void Main(string[] args)
        {
            try
            {
                var url = args[0];
                var data = args[1];
                var req = WebRequests.GetPostRequest(url, data);
                var str = req.GetResponse().GetResponseString();
                if (str != "OK")
                {
                    throw new Exception("Response from server: " + str);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                ExceptionHelpers.IgnoreException(
                    () => File.WriteAllText("./ErrorReporterLog.txt", e.ToString()));
                Environment.Exit(1);
            }

            Environment.Exit(0);
        }
    }
}
