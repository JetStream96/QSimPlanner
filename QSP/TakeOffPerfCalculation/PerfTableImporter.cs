using System;
using System.Linq;
using System.Xml.Linq;
using QSP.MathTools;
using static QSP.AviationTools.AviationConstants;
using static QSP.LibraryExtension.Arrays;

namespace QSP.TakeOffPerfCalculation
{
    public static class TOPerfImporter
    {
        // Corresponding names in xml file.
        private const string PacksOffDry = "PacksOffDry";
        private const string PacksOffWet = "PacksOffWet";
        private const string PacksOffClimb = "PacksOffClimb";
        private const string AIBothDry = "AIBothDry";
        private const string AIBothWet = "AIBothWet";
        private const string AIBothClimb = "AIBothClimb";
        private const string AIEngDry = "AIEngDry";
        private const string AIEngWet = "AIEngWet";
        private const string AIEngClimb = "AIEngClimb";

        private static char[] CHANGELINE = { '\r', '\n' };
        private static char[] EMPTYSPACE = { ' ', '\t' };

        /// <summary>
        /// Load an aircraft data from specified Xml file.
        /// </summary>
        public static void ReadFromXml(AircraftParameters para, string filepath)
        {
            XDocument doc = XDocument.Load(filepath);
            var root = doc.Root;
            bool wtUnitIsKG = false;
            bool lengthUnitIsMeter = false;

            //read weight/length units
            if (root.Element("WeightUnit").Value == "1000KG")
            {
                wtUnitIsKG = true;
            }
            else
            {
                wtUnitIsKG = false;
            }

            if (root.Element("LengthUnit").Value == "M")
            {
                lengthUnitIsMeter = true;
            }
            else
            {
                lengthUnitIsMeter = false;
            }

            //read adjustment part
            var packs = root.Element("Adjustments").Element("Packs");
            para.PacksOffDry = Convert.ToDouble(packs.Element("Dry").Value);
            para.PacksOffWet = Convert.ToDouble(packs.Element("Wet").Value);
            para.PacksOffClimb = Convert.ToDouble(packs.Element("Climb").Value);

            var antiIceEng = root.Element("Adjustments").Element("AntiIce").Element("EngineOnly");
            para.AIEngDry = Convert.ToDouble(antiIceEng.Element("Dry").Value);
            para.AIEngWet = Convert.ToDouble(antiIceEng.Element("Wet").Value);
            para.AIEngClimb = Convert.ToDouble(antiIceEng.Element("Climb").Value);

            var antiIceBoth = root.Element("Adjustments").Element("AntiIce").Element("EngineAndWing");
            para.AIBothDry = Convert.ToDouble(antiIceBoth.Element("Dry").Value);
            para.AIBothWet = Convert.ToDouble(antiIceBoth.Element("Wet").Value);
            para.AIBothClimb = Convert.ToDouble(antiIceBoth.Element("Climb").Value);

            //Import tables
            var dryNode = root.Element("Dry");
            para.SlopeCorrDry = getSlopeCorr(dryNode.Element("SlopeCorrection").Value, lengthUnitIsMeter);
            para.WindCorrDry = getSlopeCorr(dryNode.Element("WindCorrection").Value, lengthUnitIsMeter);
            var tables = getFieldClimbLimitWt(dryNode, lengthUnitIsMeter, wtUnitIsKG);
            para.WeightTableDry = tables.Item1;
            para.ClimbLimitWt = tables.Item2;

            var wetNode = root.Element("Wet");
            para.SlopeCorrWet = getSlopeCorr(wetNode.Element("SlopeCorrection").Value, lengthUnitIsMeter);
            para.WindCorrWet = getSlopeCorr(wetNode.Element("WindCorrection").Value, lengthUnitIsMeter);
            para.WeightTableWet = getFieldClimbLimitWt(wetNode, lengthUnitIsMeter, wtUnitIsKG).Item1;

            // TO1, TO2
            var altnRating = importAltnRating(root, wtUnitIsKG);
            if (altnRating == null)
            {
                para.AltnRatingAvail = false;
                para.ThrustRatings = new ThrustRatingOption[] { ThrustRatingOption.Normal };
            }
            else
            {
                para.AltnRatingAvail = true;
                para.AlternateThrustRating = altnRating;
                para.ThrustRatings = new ThrustRatingOption[] { ThrustRatingOption.Normal, ThrustRatingOption.TO1, ThrustRatingOption.TO2 };
            }
        }

        // node should be "Dry" or "Wet" node
        private static Tuple<Table3D, Table2D> getFieldClimbLimitWt(XElement node, bool lenthIsMeter, bool WtIsKG)
        {
            var tables = node.Elements("WeightTable");

            // x
            var altitudes = new double[tables.Count()];

            //trying to get dimesions of the table
            string s = tables.First().Element("Table").Value;
            var lines = s.Split(CHANGELINE, StringSplitOptions.RemoveEmptyEntries);
            var words = lines[0].Split(EMPTYSPACE, StringSplitOptions.RemoveEmptyEntries);

            int yLen = lines.Length - 1; //Num of lengths
            int zLen = words.Length;  //Num of OAT
            var fieldLim = new double[altitudes.Length, yLen, zLen];
            var climbLim = new double[altitudes.Length, zLen];

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
                lines = s.Split(CHANGELINE, StringSplitOptions.RemoveEmptyEntries);

                for (int j = 0; j < lines.Length - 1; j++)
                {
                    words = lines[j + 1].Split(EMPTYSPACE, StringSplitOptions.RemoveEmptyEntries);

                    // Length
                    if (index == 1)
                    {
                        lengths[j] = Convert.ToDouble(words[0]);
                    }

                    for (int k = 0; k < words.Length - 1; k++)
                    {
                        fieldLim[index, j, k] = Convert.ToDouble(words[k + 1]);
                    }
                }

                // Import climbLim
                s = i.Element("Climb").Value;
                words = s.Split(EMPTYSPACE, StringSplitOptions.RemoveEmptyEntries);

                for (int m = 0; m < words.Length; m++)
                {
                    climbLim[index, m] = Convert.ToDouble(words[m]);
                }

                index++;
            }

            // Now check units
            if (!lenthIsMeter)
            {
                lengths.multiply(FT_M_ratio);
            }

            if (!WtIsKG)
            {
                climbLim.multiply(LB_KG);
                fieldLim.multiply(LB_KG);
            }

            return new Tuple<Table3D, Table2D>
                (new Table3D(altitudes, lengths, oats, fieldLim),
                 new Table2D(altitudes, oats, climbLim));

        }

        // This also works for wind correction tables.
        private static Table2D getSlopeCorr(string item, bool lengthIsMeter)
        {
            string[] lines = item.Split(CHANGELINE, StringSplitOptions.RemoveEmptyEntries);
            string[] words = lines[0].Split(EMPTYSPACE, StringSplitOptions.RemoveEmptyEntries);
            // first line is slope
            var slope = new double[words.Length];

            for (int i = 0; i < slope.Length; i++)
            {
                slope[i] = Convert.ToDouble(words[i]);
            }

            // From second line
            var lengths = new double[lines.Length - 1];
            var table = new double[lengths.Length, slope.Length];

            for (int j = 0; j < lines.Length - 1; j++)
            {
                words = lines[j + 1].Split(EMPTYSPACE, StringSplitOptions.RemoveEmptyEntries);
                lengths[j] = Convert.ToDouble(words[0]);

                for (int k = 0; k < slope.Length; k++)
                {
                    table[j,k] = Convert.ToDouble(words[k + 1]);
                }
            }

            // If length unit is feet, convert them to meter.
            if (!lengthIsMeter)
            {
                lengths.multiply(FT_M_ratio);
                table.multiply(FT_M_ratio);
            }

            return new Table2D(lengths, slope, table);
        }

        private static AlternateThrustTable[] importAltnRating(XElement rootNode, bool wtUnitIsKG)
        {
            // TO1 / TO2 
            var TO1 = rootNode.Element("TO1");
            var TO2 = rootNode.Element("TO2");

            if (TO1 == null || TO2 == null)
            {
                return null;
            }
            return new AlternateThrustTable[] {
                loadAltnRatingTable(TO1.Value, wtUnitIsKG), loadAltnRatingTable(TO2.Value, wtUnitIsKG) };
        }

        private static void fillArray(string[] item, double[] array)
        {
            for (int i = 0; i < item.Length; i++)
            {
                array[i] = Convert.ToDouble(item[i]);
            }
        }

        private static AlternateThrustTable loadAltnRatingTable(string item, bool wtUnitIsKG)
        {
            var lines = item.Split(CHANGELINE, StringSplitOptions.RemoveEmptyEntries);
            string[] words = lines[0].Split(EMPTYSPACE, StringSplitOptions.RemoveEmptyEntries);
            int LEN = words.Length;
            int flag = 0;

            var fullThrust = new double[LEN];
            var dry = new double[LEN];
            var wet = new double[LEN];
            var climb = new double[LEN];

            foreach (string i in lines)
            {
                words = i.Split(EMPTYSPACE, StringSplitOptions.RemoveEmptyEntries);

                if (words.Length > 0 && flag <= 3)
                {
                    switch (flag)
                    {
                        case 0:
                            fillArray(words, fullThrust);
                            break;

                        case 1:
                            fillArray(words, dry);
                            break;

                        case 2:
                            fillArray(words, wet);
                            break;

                        case 3:
                            fillArray(words, climb);
                            break;
                    }
                    flag++;
                }
                else
                {
                    break;
                }
            }

            if (!wtUnitIsKG)
            {
                fullThrust.multiply(LB_KG);
                dry.multiply(LB_KG);
                wet.multiply(LB_KG);
                climb.multiply(LB_KG);
            }
            return new AlternateThrustTable(fullThrust, dry, wet, climb);
        }

    }

}
