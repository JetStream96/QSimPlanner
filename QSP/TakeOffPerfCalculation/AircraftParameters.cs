using QSP.MathTools;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace QSP.TakeOffPerfCalculation
{

    public class AircraftParameters
    {
        //All Rwy length in M
        //All Weights in KG
        //All wind spd in knots

        public double PacksOffDry;
        public double PacksOffWet;
        public double PacksOffClimb;
        public double AIBothDry;
        public double AIBothWet;
        public double AIBothClimb;
        public double AIEngDry;
        public double AIEngWet;

        public double AIEngClimb;
        public string[] AllowedFlaps;
        public bool AltnRatingAvail;
        public AlternateThrustTable[] AlternateThrustRating;

        public ThrustRatingOption[] ThrustRatings;
        public Table2D SlopeCorrDry;
        public Table2D WindCorrDry;

        public Table3D WeightTableDry;
        public Table2D SlopeCorrWet;
        public Table2D WindCorrWet;

        public Table3D WeightTableWet;
        public Table2D ClimbLimitWt;

        public AircraftParameters(Aircraft aircraft)
        {
            switch (aircraft)
            {
                case Aircraft.B737600:
                    TOPerfImporter.ReadFromXml(this, @"PerformanceData\TO\737600_TO.xml");
                    AllowedFlaps = new string[] { "5" };
                    break;

                case Aircraft.B737700:
                    TOPerfImporter.ReadFromXml(this, @"PerformanceData\TO\737700_TO.xml");
                    AllowedFlaps = new string[] { "5" };
                    break;

                case Aircraft.B737800:
                    TOPerfImporter.ReadFromXml(this, @"PerformanceData\TO\737800_TO.xml");
                    AllowedFlaps = new string[] { "5" };
                    break;

                case Aircraft.B737900:
                    TOPerfImporter.ReadFromXml(this, @"PerformanceData\TO\737900_TO.xml");
                    AllowedFlaps = new string[] { "5" };
                    break;

                case Aircraft.B777200LR:
                    TOPerfImporter.ReadFromXml(this, @"PerformanceData\TO\777200LR_TO.xml");
                    AllowedFlaps = new string[] { "15" };
                    break;

                case Aircraft.B777F:
                    TOPerfImporter.ReadFromXml(this, @"PerformanceData\TO\777F_TO.xml");
                    AllowedFlaps = new string[] { "15" };
                    break;

            }

        }


    }

}
