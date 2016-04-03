namespace QSP.LandingPerfCalculation.Boeing.PerfData
{
    // Landing performance table for various runway surface conditions, 
    // flap setting, etc.
    //
    // Every aircraft corresponds to one PerfTable.
    //
    public class BoeingPerfTable : PerfTableItem
    {
        // All lengths in meter, all weights in KG.
        public double WeightRef { get; private set; }
        public double WeightStep { get; private set; }

        private string[] autoBrkDry;
        private string[] autoBrkWet;

        public TableDry DataDry { get; private set; }
        public TableWet DataWet { get; private set; }

        public string[] BrakesAvailable(SurfaceCondition item)
        {
            if (item == SurfaceCondition.Dry)
            {
                return autoBrkDry;
            }
            else
            {
                return autoBrkWet;
            }
        }

        public string[] Flaps { get; private set; }     
        public string[] Reversers { get; private set; }
      
        public BoeingPerfTable(
            double WeightRef,
            double WeightStep,
            string[] autoBrkDry,
            string[] autoBrkWet,
            string[] Flaps,
            string[] Reversers,
            TableDry DataDry,
            TableWet DataWet)
        {
            this.WeightRef = WeightRef;
            this.WeightStep = WeightStep;
            this.autoBrkDry = autoBrkDry;
            this.autoBrkWet = autoBrkWet;
            this.Flaps = Flaps;
            this.Reversers = Reversers;
            this.DataDry = DataDry;
            this.DataWet = DataWet;
        }
    }
}
