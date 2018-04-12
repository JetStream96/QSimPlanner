using QSP.Utilities;
using System;
using System.IO;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Numbers;

namespace QSP.WindAloft
{
    public class WindFileHandler
    {
        private WxTable[] windTables;
        private FilePaths[] paths;

        public WindFileHandler()
        {
            int count = Constants.FullWindDataSet.Count;
            windTables = new WxTable[count];
            paths = GetPaths();
        }

        /// <exception cref="ReadWindFileException"></exception>
        public WxTableCollection ImportAllTables()
        {
            for (int i = 0; i < windTables.Length; i++)
            {
                var path = paths[i];
                windTables[i] = LoadFromFile(path);
            }

            return new WxTableCollection(windTables);
        }

        public void TryDeleteCsvFiles()
        {
            foreach (var i in paths)
            {
                TryDelete(i.U);
                TryDelete(i.V);
            }
        }

        private static void TryDelete(string path)
        {
            ExceptionHelpers.IgnoreException(() => File.Delete(path));
        }

        private FilePaths[] GetPaths()
        {
            // For 100mb, temp = wx1.csv, u_table = wx2.csv, v_table = wx3.csv
            // For 200mb, temp = wx4.csv, u_table = wx5.csv, v_table = wx6.csv
            // ...

            var dir = Constants.WxFileDownloadDirectory;
            var paths = new FilePaths[windTables.Length];

            for (int i = 0; i < windTables.Length; i++)
            {
                var temp = Path.Combine(dir, $"wx{i * 3 + 1}.csv");
                var pathU = Path.Combine(dir, $"wx{i * 3 + 2}.csv");
                var pathV = Path.Combine(dir, $"wx{i * 3 + 3}.csv");

                paths[i] = new FilePaths() { Temp = temp, U = pathU, V = pathV };
            }

            return paths;
        }

        private struct FilePaths { public string Temp, U, V; }

        /// <exception cref="ReadWindFileException"></exception>
        private static WxTable LoadFromFile(FilePaths p)
        {
            return new WxTable()
            {
                Temperature = new TableItem() { Values = ReadCsvIntoTable(p.Temp) },
                UWind = new TableItem() { Values = ReadCsvIntoTable(p.U) },
                VWind = new TableItem() { Values = ReadCsvIntoTable(p.V) }
            };
        }

        /// <exception cref="ReadWindFileException"></exception>
        private static double[,] ReadCsvIntoTable(string filename)
        {
            // File needs to have resolution of 1 * 1 degree.
            // The file is supposed to be .csv, for either u-comp. or v-comp.
            // This function will import either one of those.
            // Latitude is from -90 to 90.
            // Lon is from -179 to 180.

            // For convenience, let the range of longitute be -180 to 180.
            // Hence the size of returning array will be 181 * 361.

            try
            {
                var allLines = File.ReadAllLines(filename);
                double[,] table = new double[181, 361];

                // The first row is the name of the columns, which is omitted.
                for (int i = 1; i < allLines.Length; i++)
                {
                    string[] t = allLines[i].Split(',');
                    int j = RoundToInt(double.Parse(t[2])) + 90;
                    int k = RoundToInt(double.Parse(t[3])) + 180;
                    double speed = double.Parse(t[4]) / KnotMpsRatio;
                    table[j, k] = speed;
                }

                //add values for -180 (same as 180)
                for (int m = 0; m <= 180; m++)
                {
                    table[m, 0] = table[m, 360];
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new ReadWindFileException("Unable to retreive wind data from " + filename, ex);
            }
        }
    }
}
