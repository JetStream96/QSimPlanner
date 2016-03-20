using QSP.LibraryExtension;
using QSP.MathTools.Tables;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.Arrays;

namespace QSP.TOPerfCalculation.Boeing
{
    public class PerfDataLoader
    {
        private static char[] lineChangeChars = { '\r', '\n' };
        private static char[] spaceChars = { ' ', '\t' };

        private List<string> thrustRatings;
        private List<AlternateThrustTable> derateTables;

        /// <summary>
        /// Load an aircraft data from specified Xml file.
        /// </summary>
        public PerfTable ReadFromXml(string filepath)
        {
            XDocument doc = XDocument.Load(filepath);
            return new PerfTable(readTable(doc.Root), GetEntry(filepath, doc));
        }

        private BoeingPerfTable readTable(XElement root)
        {
            return new BoeingPerfTable(root.Elements("IndividualTable")
                                       .Select(x => readIndividualTable(x))
                                       .ToArray());
        }

        public static Entry GetEntry(string path, XDocument doc)
        {
            var elem = doc.Root.Element("Parameters");

            return new Entry(
                path.Substring(path.LastIndexOfAny(new char[] { '\\', '/' }) + 1), //TODO:
                elem.Element("Aircraft").Value,
                elem.Element("Description").Value,
                elem.Element("Designator").Value);
        }

        private IndividualPerfTable readIndividualTable(XElement node)
        {
            string flaps = node.Element("Flaps").Value;

            //read weight/length units
            bool wtUnitIsTon = node.Element("WeightUnit").Value == "1000KG";
            bool lengthUnitIsMeter = node.Element("LengthUnit").Value == "M";

            //read adjustment part
            var adjustments = node.Element("Adjustments");
            var packs = adjustments.Element("Packs");
            double PacksOffDry = Convert.ToDouble(packs.Element("Dry").Value);
            double PacksOffWet = Convert.ToDouble(packs.Element("Wet").Value);
            double PacksOffClimb = Convert.ToDouble(packs.Element("Climb").Value);

            var antiIceEng = adjustments.Element("AntiIce").Element("EngineOnly");
            double AIEngDry = Convert.ToDouble(antiIceEng.Element("Dry").Value);
            double AIEngWet = Convert.ToDouble(antiIceEng.Element("Wet").Value);
            double AIEngClimb = Convert.ToDouble(antiIceEng.Element("Climb").Value);

            var antiIceBoth = adjustments.Element("AntiIce").Element("EngineAndWing");
            double AIBothDry = Convert.ToDouble(antiIceBoth.Element("Dry").Value);
            double AIBothWet = Convert.ToDouble(antiIceBoth.Element("Wet").Value);
            double AIBothClimb = Convert.ToDouble(antiIceBoth.Element("Climb").Value);

            //Import tables
            var dryNode = node.Element("Dry");
            var SlopeCorrDry = getSlopeCorr(dryNode.Element("SlopeCorrection").Value, lengthUnitIsMeter);
            var WindCorrDry = getSlopeCorr(dryNode.Element("WindCorrection").Value, lengthUnitIsMeter);
            var tables = getFieldClimbLimitWt(dryNode, lengthUnitIsMeter, wtUnitIsTon);
            var WeightTableDry = tables.Item1;
            var ClimbLimitWt = tables.Item2;

            var wetNode = node.Element("Wet");
            var SlopeCorrWet = getSlopeCorr(wetNode.Element("SlopeCorrection").Value, lengthUnitIsMeter);
            var WindCorrWet = getSlopeCorr(wetNode.Element("WindCorrection").Value, lengthUnitIsMeter);
            var WeightTableWet = getFieldClimbLimitWt(wetNode, lengthUnitIsMeter, wtUnitIsTon).Item1;

            // Derates (TO1, TO2)
            importAltnRating(node, wtUnitIsTon);

            return new IndividualPerfTable(
                PacksOffDry,
                PacksOffWet,
                PacksOffClimb,
                AIBothDry,
                AIBothWet,
                AIBothClimb,
                AIEngDry,
                AIEngWet,
                AIEngClimb,
                flaps,
                thrustRatings.Count > 0,
                derateTables.ToArray(),
                thrustRatings.ToArray(),
                new SlopeCorrTable(SlopeCorrDry),
                new SlopeCorrTable(SlopeCorrWet),
                new WindCorrTable(WindCorrDry),
                new WindCorrTable(WindCorrWet),
                new FieldLimitWtTable(WeightTableDry),
                new FieldLimitWtTable(WeightTableWet),
                new ClimbLimitWtTable(ClimbLimitWt));
        }

        // node should be "Dry" or "Wet" node
        private static Tuple<Table3D, Table2D> getFieldClimbLimitWt(XElement node, bool lenthIsMeter, bool WtIsKG)
        {
            var tables = node.Elements("WeightTable");

            // x
            var altitudes = new double[tables.Count()];

            //trying to get dimesions of the table
            string s = tables.First().Element("Table").Value;
            var lines = s.Split(lineChangeChars, StringSplitOptions.RemoveEmptyEntries);
            var words = lines[0].Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);

            int yLen = lines.Length - 1; //Num of lengths
            int zLen = words.Length;  //Num of OAT
            var fieldLim = JaggedArrays.CreateJaggedArray<double[][][]>(altitudes.Length, yLen, zLen);
            var climbLim = JaggedArrays.CreateJaggedArray<double[][]>(altitudes.Length, zLen);

            // First line is OAT
            var oats = new double[zLen];
            for (int i = 0; i < oats.Length; i++)
            {
                oats[i] = Convert.ToDouble(words[i]);
            }

            var lengths = new double[yLen];

            int index = 0;
            foreach (var i in tables)
            {
                altitudes[index] = Convert.ToDouble(i.Element("Altitude").Value);

                //Import fieldLim
                s = i.Element("Table").Value;
                lines = s.Split(lineChangeChars, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < lines.Length - 1; j++)
                {
                    words = lines[j + 1].Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);

                    // Length
                    if (index == 1)
                    {
                        lengths[j] = Convert.ToDouble(words[0]);
                    }

                    for (int k = 0; k < words.Length - 1; k++)
                    {
                        fieldLim[index][j][k] = Convert.ToDouble(words[k + 1]);
                    }
                }

                // Import climbLim
                s = i.Element("Climb").Value;
                words = s.Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);

                for (int m = 0; m < words.Length; m++)
                {
                    climbLim[index][m] = Convert.ToDouble(words[m]);
                }

                index++;
            }

            // Now check units
            if (lenthIsMeter == false)
            {
                lengths.Multiply(FtMeterRatio);
            }

            if (WtIsKG == false)
            {
                climbLim.Multiply(LbKgRatio);
                fieldLim.Multiply(LbKgRatio);
            }

            return new Tuple<Table3D, Table2D>(
                 new Table3D(altitudes, lengths, oats, fieldLim),
                 new Table2D(altitudes, oats, climbLim));

        }

        // This also works for wind correction tables.
        private static Table2D getSlopeCorr(string item, bool lengthIsMeter)
        {
            string[] lines = item.Split(lineChangeChars, StringSplitOptions.RemoveEmptyEntries);
            string[] words = lines[0].Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);
            // first line is slope
            var slope = new double[words.Length];

            for (int i = 0; i < slope.Length; i++)
            {
                slope[i] = Convert.ToDouble(words[i]);
            }

            // From second line
            var lengths = new double[lines.Length - 1];
            var table = JaggedArrays.CreateJaggedArray<double[][]>(lengths.Length, slope.Length);

            for (int j = 0; j < lines.Length - 1; j++)
            {
                words = lines[j + 1].Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);
                lengths[j] = Convert.ToDouble(words[0]);

                for (int k = 0; k < slope.Length; k++)
                {
                    table[j][k] = Convert.ToDouble(words[k + 1]);
                }
            }

            // If length unit is feet, convert them to meter.
            if (lengthIsMeter == false)
            {
                lengths.Multiply(FtMeterRatio);
                table.Multiply(FtMeterRatio);
            }

            return new Table2D(lengths, slope, table);
        }

        private void importAltnRating(XElement individualNode, bool wtUnitIsKG)
        {
            thrustRatings = new List<string>();
            derateTables = new List<AlternateThrustTable>();
            var elements = individualNode.Element("Derates")?.Elements();

            if (elements != null)
            {
                foreach (var i in elements)
                {
                    if (i.Name.ToString() == "FullThrustName")
                    {
                        thrustRatings.Insert(0, i.Value);
                    }
                    else
                    {
                        thrustRatings.Add(i.Name.ToString());
                        derateTables.Add(loadAltnRatingTable(i.Value, wtUnitIsKG));
                    }
                }
            }
        }

        private static AlternateThrustTable loadAltnRatingTable(string item, bool wtUnitIsKG)
        {
            var lines = item.Split(lineChangeChars, StringSplitOptions.RemoveEmptyEntries);
            string[] words = lines[0].Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);
            int len = words.Length;
            int flag = 0;

            var fullThrust = new double[len];
            var dry = new double[len];
            var wet = new double[len];
            var climb = new double[len];

            foreach (string i in lines)
            {
                words = i.Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 0 && flag <= 3)
                {
                    switch (flag)
                    {
                        case 0:
                            fullThrust = words.Select(x => Convert.ToDouble(x)).ToArray();
                            break;

                        case 1:
                            dry = words.Select(x => Convert.ToDouble(x)).ToArray();
                            break;

                        case 2:
                            wet = words.Select(x => Convert.ToDouble(x)).ToArray();
                            break;

                        case 3:
                            climb = words.Select(x => Convert.ToDouble(x)).ToArray();
                            break;
                    }
                    flag++;
                }
                else
                {
                    break;
                }
            }

            if (wtUnitIsKG == false)
            {
                fullThrust.Multiply(LbKgRatio);
                dry.Multiply(LbKgRatio);
                wet.Multiply(LbKgRatio);
                climb.Multiply(LbKgRatio);
            }
            return new AlternateThrustTable(fullThrust, dry, wet, climb);
        }
    }
}
