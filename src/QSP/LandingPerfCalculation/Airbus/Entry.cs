using QSP.MathTools.Tables;

namespace QSP.LandingPerfCalculation.Airbus
{
    public class Entry
    {
        public Table1D Dry { get; set; }
        public Table1D Wet { get; set; }
        public string Flaps { get; set; }
        public string Autobrake { get; set; }
        public double ElevationDry { get; set; }
        public double ElevationWet { get; set; }
        public double HeadwindDry { get; set; }
        public double HeadwindWet { get; set; }
        public double TailwindDry { get; set; }
        public double TailwindWet { get; set; }
        public double BothReversersDry { get; set; }
        public double BothReversersWet { get; set; }
        public double Speed5Knots { get; set; }
    }
}