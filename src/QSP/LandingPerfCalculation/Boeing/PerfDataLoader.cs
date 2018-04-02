using QSP.LandingPerfCalculation.Boeing.PerfData;
using QSP.LibraryExtension;
using QSP.LibraryExtension.JaggedArrays;
using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.Arrays;
using static QSP.LibraryExtension.XmlSerialization.SerializationHelper;

namespace QSP.LandingPerfCalculation.Boeing
{
    public class PerfDataLoader
    {
        private const int ColumnCount = 14;
        private const int SurfaceCondCount = 3;

        public PerfTable ReadFromXml(string filePath)
        {
            var doc = XDocument.Load(filePath);
            return new PerfTable(LoadTable(filePath), LdgTableLoader.GetEntry(filePath, doc));
        }

        private BoeingPerfTable LoadTable(string path)
        {
            var doc = XDocument.Load(path);
            var root = doc.Root;
            var loc = root.Element("FileLocation");
            if (loc != null) return LoadTable(Path.Combine(path, "..", loc.Value));
            return GetItem(doc);
        }

        public BoeingPerfTable GetItem(XDocument doc)
        {
            var root = doc.Root;
            var para = root.Element("Parameters");
            bool lenUnitIsMeter = para.GetString("LengthUnit") == "M";
            double wtRefKg = para.GetDouble("WeightRef");
            double wtStepKg = para.GetDouble("WeightStep");

            var brks = para.Element("Brakes");
            var brkSettingDry = brks.GetString("Dry").Split(';');
            var brkSettingWet = brks.GetString("Wet").Split(';');
            var reversers = para.GetString("Reversers").Split(';');

            var data = root.Elements("Data").ToArray();
            int flapsCount = data.Length;
            string[] flaps = new string[flapsCount];

            var tableDry = JaggedArray.Create<double[][][]>
                (flapsCount, brkSettingDry.Length, ColumnCount);

            var tableWet = JaggedArray.Create<double[][][][]>
                (flapsCount, SurfaceCondCount, brkSettingWet.Length, ColumnCount);

            for (int i = 0; i < flapsCount; i++)
            {
                flaps[i] = data[i].Element("Flaps").Value;
                ReadTableDry(tableDry, i, data[i].GetString("Dry"));

                ReadTableWet(tableWet, i, 0, data[i].GetString("Good"));
                ReadTableWet(tableWet, i, 1, data[i].GetString("Medium"));
                ReadTableWet(tableWet, i, 2, data[i].GetString("Poor"));
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

        private void ReadTableDry(double[][][] item, int firstIndex, string value)
        {
            string[] lines = value.Split(new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            int brakeSettingCount = Math.Min(item[0].Length, lines.Length);

            for (int i = 0; i < brakeSettingCount; i++)
            {
                var words = lines[i].Split(new[] { ' ', '\t', '/' },
                    StringSplitOptions.RemoveEmptyEntries);

                int columnCount = Math.Min(item[0][0].Length, words.Length);

                for (int j = 0; j < columnCount; j++)
                {
                    item[firstIndex][i][j] = Convert.ToDouble(words[j]);
                }
            }
        }

        private void ReadTableWet(double[][][][] item,
            int firstIndex, int secondIndex, string value)
        {
            string[] lines = value.Split(new[] { '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            int brakeSettingCount = Math.Min(item[0][0].Length, lines.Length);

            for (int i = 0; i < brakeSettingCount; i++)
            {
                var words = lines[i].Split(new[] { ' ', '\t', '/' },
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
