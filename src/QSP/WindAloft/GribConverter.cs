using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace QSP.WindAloft
{
    public static class GribConverter
    {
        private const string gribConverterName = "degrib";

        public static void ConvertGrib()
        {
            var processes = Grib2ToCsv(WindManager.DownloadFilePath);

            while (processes.Any(p => p.HasExited == false))
            {
                Thread.Sleep(100);
            }
        }

        private static List<Process> Grib2ToCsv(string filepath)
        {
            var processes = new List<Process>();

            for (int i = 1; i <= Constants.FullWindDataSet.Count * 2; i++)
            {
                var properties = new ProcessStartInfo();
                string filepathOut = filepath.Replace(".grib2", i + ".csv");

                properties.FileName = @".\Degrib\degrib.exe";

                // Command line arguments
                properties.Arguments =
                    $"-in {filepath} -C -msg {i} -Csv -out {filepathOut}";

                properties.WindowStyle = ProcessWindowStyle.Hidden;
                processes.Add(Process.Start(properties));
            }

            return processes;
        }
    }
}
