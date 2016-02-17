using System;
using System.IO;
using QSP.MathTools;
using static QSP.AviationTools.Constants;

namespace QSP
{
    public class WindTable
    {
        //The unit is knots

        private double[,] uTable = new double[181, 361];
        private double[,] vTable = new double[181, 361];

        public void LoadFromFile(string uFilePath, string vFilePath)
        {
            uTable = importWindTable(uFilePath);
            vTable = importWindTable(vFilePath);
        }

        public double GetUWind(double lat, double lon)
        {
            return getUVWindHelper(lat, lon, TableOption.U);
        }

        public double GetVWind(double lat, double lon)
        {
            return getUVWindHelper(lat, lon, TableOption.V);
        }

        public enum TableOption
        {
            U,
            V
        }

        private double getUVWindHelper(double lat, double lon, TableOption para)
        {
            int x = (int)(Math.Floor(lat));
            int y = (int)(Math.Floor(lon));

            //tricks to prevent interpolation using data that is out of range of array
            if (x == 90)
            {
                x = 89;
            }

            if (y == 180)
            {
                y = 179;
            }
            return InterpolationOld.Interpolate(x, x + 1, lat, y, y + 1, lon, getUWindHelper(x, y, para), getUWindHelper(x, y + 1, para),
                                             getUWindHelper(x + 1, y, para), getUWindHelper(x + 1, y + 1, para));
        }

        private double getUWindHelper(int lat, int lon, TableOption para)
        {
            if (para == TableOption.U)
            {
                return uTable[lat + 90, lon + 180];
            }
            else
            {
                return vTable[lat + 90, lon + 180];
            }
        }

        private double[,] importWindTable(string filename)
        {
            //file is required to have resolution of 1 degree * 1 degree
            //the file is supposed to be .csv, for either u-comp. or v-comp.
            //this function will import either one of those
            //latitude is given from -90 to 90
            //lon is given from -179 to 180

            //for convenience, let the range of longitute be -180 to 180
            //hence the size of returning array will be 181 * 361
            try
            {
                var allLines = File.ReadAllLines(filename);
                double[,] table = new double[181, 361];
                //lat, lon

                //the first row is the name of the columns, which should be omitted
                for (int i = 1; i < allLines.Length; i++)
                {
                    string[] t = allLines[i].Split(',');
                    int j = (int)(Math.Round(Convert.ToDouble(t[2])) + 90);
                    int k = (int)(Math.Round(Convert.ToDouble(t[3])) + 180);
                    double l = Convert.ToDouble(t[4]) / KT_MPS;
                    table[j, k] = l;
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
                Utilities.ErrorLogger.WriteToLog(ex);
                throw new Exception("Unable to retreive wind data from " + filename);
            }
        }
    }
}
