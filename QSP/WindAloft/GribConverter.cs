using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace QSP.WindAloft
{
    public static class GribConverter
    {
        private const string gribConverterName = "degrib";

        public static void ConvertGrib()
        {
            int numAlreadyRunning = numProcessRunning(gribConverterName);
            Grib2ToCsv(WindManager.SaveFileLocation);

            //TODO: This code is not completely error free.
            //Better to check whether all .wx files can be read.
            while ((numProcessRunning(gribConverterName) > numAlreadyRunning))
            {
                Thread.Sleep(100);
            }
        }

        private static void Grib2ToCsv(string filepath)
        {
            ProcessStartInfo ProcessProperties = new ProcessStartInfo();

            for (int i = 1; i <= Utilities.FullWindDataSet.Length * 2; i++)
            {
                string filepath_out = filepath.Replace(".grib2", i + ".csv");

                ProcessProperties.FileName = ".\\degrib\\degrib.exe";
                ProcessProperties.Arguments = "-in " + filepath + " -C -msg " + i + " -Csv -out " + filepath_out;
                //command line arguments
                ProcessProperties.WindowStyle = ProcessWindowStyle.Hidden;
                Process myProcess = Process.Start(ProcessProperties);
            }
        }

        private static int numProcessRunning(string processName)
        {
            return Process.GetProcessesByName(processName).Count();
        }

    }
}
