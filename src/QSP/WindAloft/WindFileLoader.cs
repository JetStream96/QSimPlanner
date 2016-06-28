using System;
using System.IO;
using static QSP.AviationTools.Constants;
using static QSP.MathTools.Doubles;
using static QSP.Utilities.LoggerInstance;

namespace QSP.WindAloft
{
    public class WindFileLoader
    {
        private WindTable[] windTables;

        public WindFileLoader()
        {
            int count = Utilities.FullWindDataSet.Count;
            windTables = new WindTable[count];
        }

        /// <exception cref="ReadWindFileException"></exception>
        public WindTableCollection ImportAllTables()
        {
            // For 100mb, u_table = wx1.csv, v_table = wx2.csv
            // For 200mb, u_table = wx3.csv, v_table = wx4.csv
            // ...

            for (int i = 0; i < windTables.Length; i++)
            {
                var dir = Utilities.WxFileDirectory;

                string u = dir + @"\wx" + (i * 2 + 1).ToString() + ".csv";
                string v = dir + @"\wx" + (i * 2 + 2).ToString() + ".csv";

                windTables[i] = LoadFromFile(u, v);
            }

            return new WindTableCollection(windTables);
        }

        /// <exception cref="ReadWindFileException"></exception>
        public static WindTable LoadFromFile(
            string uFilePath, string vFilePath)
        {
            var uTable = ImportWindTable(uFilePath);
            var vTable = ImportWindTable(vFilePath);

            return new WindTable(uTable, vTable);
        }

        /// <exception cref="ReadWindFileException"></exception>
        private static double[,] ImportWindTable(string filename)
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
                WriteToLog(ex);

                throw new ReadWindFileException(
                    "Unable to retreive wind data from " + filename);
            }
        }
    }
}

