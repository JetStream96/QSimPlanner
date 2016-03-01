using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension;
using System;
using System.Linq;
using System.Xml.Linq;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.Arrays;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class DataLoader
    {
        private const int ColumnCount = 14;
        private const int SurfaceCondCount = 3;

        public PerfTable ReadFromXml(string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            return new PerfTable(GetItem(doc), GetEntry(filePath, doc));
        }

        public Entry GetEntry(string path, XDocument doc)
        {
            var elem = doc.Root.Element("Parameters");

            return new Entry(
                path.Substring(path.LastIndexOfAny(new char[] { '\\', '/' }) + 1), //TODO:
                elem.Element("Aircraft").Value,
                elem.Element("Description").Value,
                elem.Element("Designator").Value);
        }

        public BoeingPerfTable GetItem(XDocument doc)
        {
            // TODO:
            var REVERSERS = new string[] { "BOTH", "ONE REV", "NO REV" };

            var root = doc.Root;
            var para = root.Element("Parameters");
            bool lenUnitIsMeter = para.Element("LengthUnit").Value == "M";
            double wtRefKg = Convert.ToDouble(para.Element("WeightRef").Value);
            double wtStepKg = Convert.ToDouble(para.Element("WeightStep").Value);

            var brks = para.Element("Autobrakes");
            string[] autoBrkDry = brks.Element("Dry").Value.Split(';');
            string[] autoBrkWet = brks.Element("Wet").Value.Split(';');

            var data = root.Elements("Data").ToArray();
            int LEN = data.Length;
            string[] flaps = new string[LEN];

            var tableDry =
                JaggedArrays.CreateJaggedArray<double[][][]>
                (LEN, autoBrkDry.Length, ColumnCount);

            var tableWet =
                JaggedArrays.CreateJaggedArray<double[][][][]>
                (LEN, SurfaceCondCount, autoBrkWet.Length, ColumnCount);

            for (int i = 0; i < LEN; i++)
            {
                flaps[i] = data[i].Element("Flaps").Value;
                readTableDry(tableDry, i, data[i].Element("Dry").Value);

                readTableWet(tableWet, i, 0, data[i].Element("Good").Value);
                readTableWet(tableWet, i, 1, data[i].Element("Medium").Value);
                readTableWet(tableWet, i, 2, data[i].Element("Poor").Value);
            }

            if (lenUnitIsMeter == false)
            {
                tableDry.Multiply(FT_M_ratio);
                tableWet.Multiply(FT_M_ratio);
            }

            return new BoeingPerfTable(
                        wtRefKg,
                        wtStepKg,
                        autoBrkDry,
                        autoBrkWet,
                        flaps,
                        REVERSERS,
                        new TableDry(tableDry),
                        new TableWet(tableWet));
        }

        private void readTableDry(double[][][] item, int firstIndex, string value)
        {
            string[] lines = value.Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            int LEN1 = Math.Min(item[0].Length, lines.Length);

            for (int i = 0; i < LEN1; i++)
            {
                var words = lines[i].Split(new char[] { ' ', '\t', '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                int LEN2 = Math.Min(item[0][0].Length, words.Length);

                for (int j = 0; j < LEN2; j++)
                {
                    item[firstIndex][i][j] = Convert.ToDouble(words[j]);
                }
            }
        }

        private void readTableWet(double[][][][] item, int firstIndex, int secondIndex, string value)
        {
            string[] lines = value.Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            int LEN1 = Math.Min(item[0][0].Length, lines.Length);

            for (int i = 0; i < LEN1; i++)
            {
                var words = lines[i].Split(new char[] { ' ', '\t', '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                int LEN2 = Math.Min(item[0][0][0].Length, words.Length);
                for (int j = 0; j < LEN2; j++)
                {
                    item[firstIndex][secondIndex][i][j] = Convert.ToDouble(words[j]);
                }
            }
        }
    }
}
