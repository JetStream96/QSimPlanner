using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace QSP.WindAloft
{
    public static class GribConverter
    {
        public static void ConvertGrib()
        {
            var processes = Grib2ToCsv(WindManager.DownloadFilePath);

            while (processes.Any(p => !p.HasExited))
            {
                Thread.Sleep(100);
            }
        }

        private static List<Process> Grib2ToCsv(string filepath)
        {
            var processes = new List<Process>();

            for (int i = 1; i <= Constants.FullWindDataSet.Count * 3; i++)
            {
                string filepathOut = filepath.Replace(".grib2", i + ".csv");
                var properties = new ProcessStartInfo()
                {
                    FileName = @".\Degrib\degrib.exe",
                    Arguments = $"-in {filepath} -C -msg {i} -Csv -out {filepathOut}",
                    WindowStyle = ProcessWindowStyle.Hidden
                };

                processes.Add(Process.Start(properties));
            }

            return processes;
        }
    }
}
