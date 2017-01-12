using QSP.LibraryExtension;
using QSP.LibraryExtension.JaggedArrays;
using QSP.MathTools.Tables;
using QSP.MathTools.Tables.Readers;
using QSP.TOPerfCalculation.Boeing.PerfData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using QSP.LibraryExtension.XmlSerialization;
using static QSP.AviationTools.Constants;
using static QSP.LibraryExtension.Arrays;
using System.IO;

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
        public PerfTable ReadFromXml(string filePath)
        {
            var doc = XDocument.Load(filePath);
            return new PerfTable(LoadTable(filePath), GetEntry(filePath, doc));
        }

        public BoeingPerfTable ReadTable(XElement root)
        {
            return new BoeingPerfTable(root.Elements("IndividualTable")
                                       .Select(x => ReadIndividualTable(x))
                                       .ToList());
        }

        public static Entry GetEntry(string path, XDocument doc)
        {
            var elem = doc.Root.Element("Parameters");
            return new Entry(elem.Element("ProfileName").Value, path);
        }

        private BoeingPerfTable LoadTable(string path)
        {
            var root = XDocument.Load(path).Root;
            var loc = root.Element("FileLocation");
            if (loc != null) return LoadTable(Path.Combine(path, "..", loc.Value));
            return ReadTable(root);
        }

        private IndividualPerfTable ReadIndividualTable(XElement node)
        {
            string flaps = node.Element("Flaps").Value;

            //read weight/length units
            bool wtUnitIsTon = node.Element("WeightUnit").Value == "1000KG";
            bool lengthUnitIsMeter = node.Element("LengthUnit").Value == "M";

            //read adjustment part
            var adjustments = node.Element("Adjustments");
            var packs = adjustments.Element("Packs");
            double PacksOffDry = packs.GetDouble("Dry");
            double PacksOffWet = packs.GetDouble("Wet");
            double PacksOffClimb = packs.GetDouble("Climb");

            var antiIceEng = adjustments.Element("AntiIce").Element("EngineOnly");
            double AIEngDry = antiIceEng.GetDouble("Dry");
            double AIEngWet = antiIceEng.GetDouble("Wet");
            double AIEngClimb = antiIceEng.GetDouble("Climb");

            var antiIceBoth = adjustments.Element("AntiIce").Element("EngineAndWing");
            double AIBothDry = antiIceBoth.GetDouble("Dry");
            double AIBothWet = antiIceBoth.GetDouble("Wet");
            double AIBothClimb = antiIceBoth.GetDouble("Climb");

            //Import tables
            var dryNode = node.Element("Dry");
            var wetNode = node.Element("Wet");

            var slopeCorrDry = TableReader2D.Read(dryNode.Element("SlopeCorrection").Value);
            var windCorrDry = TableReader2D.Read(dryNode.Element("WindCorrection").Value);
            var SlopeCorrWet = TableReader2D.Read(wetNode.Element("SlopeCorrection").Value);
            var WindCorrWet = TableReader2D.Read(wetNode.Element("WindCorrection").Value);

            SetUnitSlopeOrWindTable(slopeCorrDry, lengthUnitIsMeter);
            SetUnitSlopeOrWindTable(windCorrDry, lengthUnitIsMeter);
            SetUnitSlopeOrWindTable(SlopeCorrWet, lengthUnitIsMeter);
            SetUnitSlopeOrWindTable(WindCorrWet, lengthUnitIsMeter);

            var tables = SetFieldClimbLimitWt(dryNode, lengthUnitIsMeter, wtUnitIsTon);
            var WeightTableDry = tables.Field;
            var ClimbLimitWt = tables.Climb;

            var WeightTableWet = SetFieldClimbLimitWt(wetNode, lengthUnitIsMeter, wtUnitIsTon).Field;

            // Derates (TO1, TO2)
            ImportAltnRating(node, wtUnitIsTon);

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
                derateTables,
                thrustRatings,
                new SlopeCorrTable(slopeCorrDry.x, slopeCorrDry.y, slopeCorrDry.f),
                new SlopeCorrTable(SlopeCorrWet.x, SlopeCorrWet.y, SlopeCorrWet.f),
                new WindCorrTable(windCorrDry.x, windCorrDry.y, windCorrDry.f),
                new WindCorrTable(WindCorrWet.x, WindCorrWet.y, WindCorrWet.f),
                WeightTableDry,
                WeightTableWet,
                ClimbLimitWt);
        }

        // node should be "Dry" or "Wet" node
        private static WtTables SetFieldClimbLimitWt(XElement node, bool lenthIsMeter, bool WtIsKG)
        {
            var wtTables = node.Elements("WeightTable").ToList();

            // x
            var altitudes = wtTables.Select(x => x.Element("Altitude").Value)
                .ToDoubles();

            var fieldLimTables = wtTables
                .Select(x => TableReader2D.Read(x.Element("Table").Value))
                .ToList();

            var lengths = fieldLimTables.First().x;
            var oats = fieldLimTables.First().y;
            var fieldlimitWt = fieldLimTables.Select(t => t.f).ToArray();

            double[][] climbLimWt = GetClimbLimit(wtTables);

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

            return new WtTables(
                 new FieldLimitWtTable(altitudes, lengths, oats, fieldlimitWt),
                 new ClimbLimitWtTable(altitudes, oats, climbLimWt));
        }

        private static double[][] GetClimbLimit(IEnumerable<XElement> elem)
        {
            return elem.Select(x => 
                x.Element("Climb")
                 .Value
                 .Split(spaceChars, StringSplitOptions.RemoveEmptyEntries)
                 .ToDoubles()).ToArray();
        }

        private static void SetUnitSlopeOrWindTable(Table2D table, bool lengthIsMeter)
        {
            // If length unit is feet, convert them to meter.
            if (lengthIsMeter == false)
            {
                table.f.Multiply(FtMeterRatio);
                table.x.Multiply(FtMeterRatio);
            }
        }

        private void ImportAltnRating(XElement individualNode, bool wtUnitIsKG)
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
                        derateTables.Add(LoadAltnRatingTable(i.Value, wtUnitIsKG));
                    }
                }
            }
        }

        private static double[] ConvertToArray(string line)
        {
            return line.Split(spaceChars, StringSplitOptions.RemoveEmptyEntries).ToDoubles();
        }

        private static AlternateThrustTable LoadAltnRatingTable(string item, bool wtUnitIsKG)
        {
            var lines = item.Split(lineChangeChars, StringSplitOptions.RemoveEmptyEntries);
            var fullThrust = ConvertToArray(lines[0]);
            var dry = ConvertToArray(lines[1]);
            var wet = ConvertToArray(lines[2]);
            var climb = ConvertToArray(lines[3]);
            
            if (wtUnitIsKG == false)
            {
                fullThrust.Multiply(LbKgRatio);
                dry.Multiply(LbKgRatio);
                wet.Multiply(LbKgRatio);
                climb.Multiply(LbKgRatio);
            }

            return new AlternateThrustTable(fullThrust, dry, wet, climb);
        }

        public struct WtTables
        {
            public FieldLimitWtTable Field { get; }
            public ClimbLimitWtTable Climb { get; }

            public WtTables(FieldLimitWtTable Field, ClimbLimitWtTable Climb)
            {
                this.Field = Field;
                this.Climb = Climb;
            }
        }
    }
}
