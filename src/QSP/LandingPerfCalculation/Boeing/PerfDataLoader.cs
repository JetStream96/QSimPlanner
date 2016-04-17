using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension;
using QSP.LibraryExtension.JaggedArrays;
using System;
using System.Linq;
using System.Xml.Linq;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.Arrays;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class PerfDataLoader
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
                elem.Element("Aircraft").Value,
                elem.Element("ProfileName").Value,
                elem.Element("Designator").Value);
        }

        public BoeingPerfTable GetItem(XDocument doc)
        {
            var root = doc.Root;
            var para = root.Element("Parameters");
            bool lenUnitIsMeter = para.Element("LengthUnit").Value == "M";
            double wtRefKg = Convert.ToDouble(para.Element("WeightRef").Value);
            double wtStepKg = Convert.ToDouble(para.Element("WeightStep").Value);

            var brks = para.Element("Brakes");
            var brkSettingDry = brks.Element("Dry").Value.Split(';');
            var brkSettingWet = brks.Element("Wet").Value.Split(';');
            var reversers = para.Element("Reversers").Value.Split(';');

            var data = root.Elements("Data").ToArray();
            int flapsCount = data.Length;
            string[] flaps = new string[flapsCount];

            var tableDry =
                JaggedArray.Create<double[][][]>
                (flapsCount, brkSettingDry.Length, ColumnCount);

            var tableWet =
                JaggedArray.Create<double[][][][]>
                (flapsCount, SurfaceCondCount, brkSettingWet.Length, ColumnCount);

            for (int i = 0; i < flapsCount; i++)
            {
                flaps[i] = data[i].Element("Flaps").Value;
                readTableDry(tableDry, i, data[i].Element("Dry").Value);

                readTableWet(tableWet, i, 0, data[i].Element("Good").Value);
                readTableWet(tableWet, i, 1, data[i].Element("Medium").Value);
                readTableWet(tableWet, i, 2, data[i].Element("Poor").Value);
            }

            if (lenUnitIsMeter == false)
            {
                tableDry.Multiply(FtMeterRatio);
                tableWet.Multiply(FtMeterRatio);
            }

            return new BoeingPerfTable(
                        wtRefKg,
                        wtStepKg,
                        brkSettingDry,
                        brkSettingWet,
                        flaps,
                        reversers,
                        new TableDry(tableDry),
                        new TableWet(tableWet));
        }

        private void readTableDry(double[][][] item, int firstIndex, string value)
        {
            string[] lines = value.Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            int brakeSettingCount = Math.Min(item[0].Length, lines.Length);

            for (int i = 0; i < brakeSettingCount; i++)
            {
                var words = lines[i].Split(new char[] { ' ', '\t', '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                int columnCount = Math.Min(item[0][0].Length, words.Length);

                for (int j = 0; j < columnCount; j++)
                {
                    item[firstIndex][i][j] = Convert.ToDouble(words[j]);
                }
            }
        }

        private void readTableWet(double[][][][] item, int firstIndex, int secondIndex, string value)
        {
            string[] lines = value.Split(new char[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            int brakeSettingCount = Math.Min(item[0][0].Length, lines.Length);

            for (int i = 0; i < brakeSettingCount; i++)
            {
                var words = lines[i].Split(new char[] { ' ', '\t', '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                int columnCount = Math.Min(item[0][0][0].Length, words.Length);
                for (int j = 0; j < columnCount; j++)
                {
                    item[firstIndex][secondIndex][i][j] = Convert.ToDouble(words[j]);
                }
            }
        }
    }
}
