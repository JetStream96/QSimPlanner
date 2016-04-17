using QSP.LibraryExtension;
using QSP.LibraryExtension.JaggedArrays;
using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;
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
            return new PerfTable(ReadTable(doc.Root), GetEntry(filepath, doc));
        }

        public BoeingPerfTable ReadTable(XElement root)
        {
            return new BoeingPerfTable(root.Elements("IndividualTable")
                                       .Select(x => readIndividualTable(x))
                                       .ToArray());
        }

        public static Entry GetEntry(string path, XDocument doc)
        {
            var elem = doc.Root.Element("Parameters");

            return new Entry(
                elem.Element("Aircraft").Value, 
                elem.Element("ProfileName").Value,
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
            var wetNode = node.Element("Wet");

            var slopeCorrDry = TableReader2D.Read(dryNode.Element("SlopeCorrection").Value);
            var windCorrDry = TableReader2D.Read(dryNode.Element("WindCorrection").Value);
            var SlopeCorrWet = TableReader2D.Read(wetNode.Element("SlopeCorrection").Value);
            var WindCorrWet = TableReader2D.Read(wetNode.Element("WindCorrection").Value);

            setUnitSlopeOrWindTable(slopeCorrDry, lengthUnitIsMeter);
            setUnitSlopeOrWindTable(windCorrDry, lengthUnitIsMeter);
            setUnitSlopeOrWindTable(SlopeCorrWet, lengthUnitIsMeter);
            setUnitSlopeOrWindTable(WindCorrWet, lengthUnitIsMeter);

            var tables = setFieldClimbLimitWt(dryNode, lengthUnitIsMeter, wtUnitIsTon);
            var WeightTableDry = tables.Item1;
            var ClimbLimitWt = tables.Item2;

            var WeightTableWet = setFieldClimbLimitWt(wetNode, lengthUnitIsMeter, wtUnitIsTon).Item1;

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
                new SlopeCorrTable(slopeCorrDry.x, slopeCorrDry.y, slopeCorrDry.f),
                new SlopeCorrTable(SlopeCorrWet.x, SlopeCorrWet.y, SlopeCorrWet.f),
                new WindCorrTable(windCorrDry.x, windCorrDry.y, windCorrDry.f),
                new WindCorrTable(WindCorrWet.x, WindCorrWet.y, WindCorrWet.f),
                WeightTableDry,
                WeightTableWet,
                ClimbLimitWt);
        }

        // node should be "Dry" or "Wet" node
        private static Pair<FieldLimitWtTable, ClimbLimitWtTable> setFieldClimbLimitWt(
            XElement node, bool lenthIsMeter, bool WtIsKG)
        {
            var wtTables = node.Elements("WeightTable");

            // x
            var altitudes = wtTables.Select(x => x.Element("Altitude").Value)
                .ToDoubles();

            var fieldLimTables = wtTables.Select(
                x => TableReader2D.Read(x.Element("Table").Value));

            var lengths = fieldLimTables.First().x;
            var oats = fieldLimTables.First().y;
            var fieldlimitWt = fieldLimTables.Select(t => t.f).ToArray();

            double[][] climbLimWt = getClimbLimit(wtTables);

            // Now check units
            if (lenthIsMeter == false)
            {
                lengths.Multiply(FtMeterRatio);
            }

            if (WtIsKG == false)
            {
                climbLimWt.Multiply(LbKgRatio);
                fieldlimitWt.Multiply(LbKgRatio);
            }

            return new Pair<FieldLimitWtTable, ClimbLimitWtTable>(
                 new FieldLimitWtTable(altitudes, lengths, oats, fieldlimitWt),
                 new ClimbLimitWtTable(altitudes, oats, climbLimWt));
        }

        private static double[][] getClimbLimit(IEnumerable<XElement> elem)
        {
            return elem.Select(
                        x => x.Element("Climb")
                                .Value
                                .Split(spaceChars,
                                        StringSplitOptions.RemoveEmptyEntries)
                                .ToDoubles())
                            .ToArray();
        }
        
        private static void setUnitSlopeOrWindTable(Table2D table, bool lengthIsMeter)
        {
            // If length unit is feet, convert them to meter.
            if (lengthIsMeter == false)
            {
                table.f.Multiply(FtMeterRatio);
                table.x.Multiply(FtMeterRatio);
            }
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
            int flag = 0;

            double[] fullThrust = null;
            double[] dry = null;
            double[] wet = null;
            double[] climb = null;

            foreach (string i in lines)
            {
                var words = i.Split(spaceChars, StringSplitOptions.RemoveEmptyEntries);

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
