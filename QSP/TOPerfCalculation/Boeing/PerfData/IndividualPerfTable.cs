namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class IndividualPerfTable
    {
        // All Rwy lengths in meter.
        // All Weights in KG
        // All wind speeds in knots.

        // Corrections for limit weights.
        // All in KG.
        public double PacksOffDry { get; private set; }
        public double PacksOffWet { get; private set; }
        public double PacksOffClimb { get; private set; }
        public double AIBothDry { get; private set; }
        public double AIBothWet { get; private set; }
        public double AIBothClimb { get; private set; }
        public double AIEngDry { get; private set; }
        public double AIEngWet { get; private set; }
        public double AIEngClimb { get; private set; }
        public string Flaps { get; private set; }
        public bool AltnRatingAvail { get; private set; }
        public AlternateThrustTable[] AlternateThrustTables { get; private set; }
        public string[] ThrustRatings { get; private set; }
        public SlopeCorrTable SlopeCorrDry { get; private set; }
        public SlopeCorrTable SlopeCorrWet { get; private set; }
        public WindCorrTable WindCorrDry { get; private set; }
        public WindCorrTable WindCorrWet { get; private set; }
        public FieldLimitWtTable WeightTableDry { get; private set; }
        public FieldLimitWtTable WeightTableWet { get; private set; }
        public ClimbLimitWtTable ClimbLimitWt { get; private set; }

        public IndividualPerfTable(
             double PacksOffDry,
             double PacksOffWet,
             double PacksOffClimb,
             double AIBothDry,
             double AIBothWet,
             double AIBothClimb,
             double AIEngDry,
             double AIEngWet,
             double AIEngClimb,
             string Flaps,
             bool AltnRatingAvail,
             AlternateThrustTable[] AlternateThrustTables,
             string[] ThrustRatings,
             SlopeCorrTable SlopeCorrDry,
             SlopeCorrTable SlopeCorrWet,
             WindCorrTable WindCorrDry,
             WindCorrTable WindCorrWet,
             FieldLimitWtTable WeightTableDry,
             FieldLimitWtTable WeightTableWet,
             ClimbLimitWtTable ClimbLimitWt)
        {
            this.PacksOffDry = PacksOffDry;
            this.PacksOffWet = PacksOffWet;
            this.PacksOffClimb = PacksOffClimb;
            this.AIBothDry = AIBothDry;
            this.AIBothWet = AIBothWet;
            this.AIBothClimb = AIBothClimb;
            this.AIEngDry = AIEngDry;
            this.AIEngWet = AIEngWet;
            this.AIEngClimb = AIEngClimb;
            this.Flaps = Flaps;
            this.AltnRatingAvail = AltnRatingAvail;
            this.AlternateThrustTables = AlternateThrustTables;
            this.ThrustRatings = ThrustRatings;
            this.SlopeCorrDry = SlopeCorrDry;
            this.SlopeCorrWet = SlopeCorrWet;
            this.WindCorrDry = WindCorrDry;
            this.WindCorrWet = WindCorrWet;
            this.WeightTableDry = WeightTableDry;
            this.WeightTableWet = WeightTableWet;
            this.ClimbLimitWt = ClimbLimitWt;
        }
    }
}
