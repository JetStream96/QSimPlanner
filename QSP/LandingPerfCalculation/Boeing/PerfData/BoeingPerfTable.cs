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
        public double weightRef { get; private set; }
        public double weightStep { get; private set; }

        private string[] autoBrkDry;
        private string[] autoBrkWet;
        private string[] flaps;
        private string[] reversers;

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

        public string[] FlapsAvailable()
        {
            return flaps;
        }

        public string[] RevAvailable()
        {
            return reversers;
        }

        public BoeingPerfTable(
            double weightRef,
            double weightStep,
            string[] autoBrkDry,
            string[] autoBrkWet,
            string[] flaps,
            string[] reversers,
            TableDry DataDry,
            TableWet DataWet)
        {
            this.weightRef = weightRef;
            this.weightStep = weightStep;
            this.autoBrkDry = autoBrkDry;
            this.autoBrkWet = autoBrkWet;
            this.flaps = flaps;
            this.reversers = reversers;
            this.DataDry = DataDry;
            this.DataWet = DataWet;
        }
    }
}
