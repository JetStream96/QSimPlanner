using System.Collections.Generic;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class IndividualPerfTable
    {
        // All Rwy lengths in meter.
        // In tables, all Weights are in ton.
        // All wind speeds in knots.

        // Corrections for limit weights.
        // All in KG.
        public double PacksOffDry { get; }
        public double PacksOffWet { get; }
        public double PacksOffClimb { get; }
        public double AIBothDry { get; }
        public double AIBothWet { get; }
        public double AIBothClimb { get; }
        public double AIEngDry { get; }
        public double AIEngWet { get; }
        public double AIEngClimb { get; }
        //

        public string Flaps { get; }
        public bool AltnRatingAvail { get; }
        public IReadOnlyList<AlternateThrustTable> AlternateThrustTables { get; }
        public IReadOnlyList<string> ThrustRatings { get; }

        public SlopeCorrTable SlopeCorrDry { get; }
        public SlopeCorrTable SlopeCorrWet { get; }
        public WindCorrTable WindCorrDry { get; }
        public WindCorrTable WindCorrWet { get; }
        public FieldLimitWtTable WeightTableDry { get; }
        public FieldLimitWtTable WeightTableWet { get; }
        public ClimbLimitWtTable ClimbLimitWt { get; }

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
             IReadOnlyList<AlternateThrustTable> AlternateThrustTables,
             IReadOnlyList<string> ThrustRatings,
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
