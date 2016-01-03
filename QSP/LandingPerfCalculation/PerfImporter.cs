using System;
using System.Linq;
using System.Xml.Linq;
using static QSP.LibraryExtension.Arrays;
using static QSP.AviationTools.AviationConstants;

namespace QSP.LandingPerfCalculation
{
    public static class PerfImporter
    {
        public static PerfData ReadFromXml(string filePath)
        {
            const int COL_NUM = 14;
            const int NUM_SURF_CON = 3;
            var REVERSERS = new string[] { "BOTH", "ONE REV", "NO REV" };

            XDocument doc = XDocument.Load(filePath);
            var root = doc.Root;
            var para = root.Element("Pamameters");
            bool lenUnitIsMeter = para.Element("LengthUnit").Value == "M" ? true : false;
            double wtRefKg = Convert.ToDouble(para.Element("WeightRef").Value);
            double wtStepKg = Convert.ToDouble(para.Element("WeightStep").Value);

            var brks = para.Element("Autobrakes");
            string[] autoBrkDry = brks.Element("Dry").Value.Split(';');
            string[] autoBrkWet = brks.Element("Wet").Value.Split(';');

            var data = root.Elements("Data").ToArray();
            int LEN = data.Length;
            string[] flaps = new string[LEN];
            var tableDry = new double[LEN, autoBrkDry.Length, COL_NUM];
            var tableWet = new double[LEN, NUM_SURF_CON, autoBrkWet.Length, COL_NUM];

            for (int i = 0; i < LEN; i++)
            {
                flaps[i] = data[i].Element("Flaps").Value;
                readTableDry(tableDry, i, data[i].Element("Dry").Value);

                readTableWet(tableWet, i, 0, data[i].Element("Good").Value);
                readTableWet(tableWet, i, 1, data[i].Element("Medium").Value);
                readTableWet(tableWet, i, 2, data[i].Element("Poor").Value);
            }

            if (!lenUnitIsMeter)
            {
                tableDry.multiply(FT_M_ratio);
                tableWet.multiply(FT_M_ratio);
            }

            return new PerfData(wtRefKg, wtStepKg, autoBrkDry, autoBrkWet, flaps, REVERSERS, tableDry, tableWet);
        }

        private static void readTableDry(double[,,] item, int firstIndex, string value)
        {
            string[] lines = value.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int LEN1 = Math.Min(item.GetLength(1), lines.Length);

            for (int i = 0; i < LEN1; i++)
            {
                var words = lines[i].Split(new char[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);

                int LEN2 = Math.Min(item.GetLength(2), words.Length);
                for (int j = 0; j < LEN2; j++)
                {
                    item[firstIndex, i, j] = Convert.ToDouble(words[j]);
                }
            }
        }

        private static void readTableWet(double[,,,] item, int firstIndex, int secondIndex, string value)
        {
            string[] lines = value.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int LEN1 = Math.Min(item.GetLength(2), lines.Length);

            for (int i = 0; i < LEN1; i++)
            {
                var words = lines[i].Split(new char[] { ' ', '/' }, StringSplitOptions.RemoveEmptyEntries);

                int LEN2 = Math.Min(item.GetLength(3), words.Length);
                for (int j = 0; j < LEN2; j++)
                {
                    item[firstIndex, secondIndex, i, j] = Convert.ToDouble(words[j]);
                }
            }
        }

    }
}
