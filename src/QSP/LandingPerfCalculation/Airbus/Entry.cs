using QSP.MathTools.Tables;
using static System.Math;

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

        public bool Equals(Entry e, double delta)
        {
            return e != null &&
                Dry != null && Dry.Equals(e.Dry, delta) &&
                Wet != null && Wet.Equals(e.Wet, delta) &&
                Flaps == e.Flaps &&
                Autobrake == e.Autobrake &&
                Abs(ElevationDry - e.ElevationDry) < delta &&
                Abs(ElevationWet - e.ElevationWet) < delta &&
                Abs(HeadwindDry - e.HeadwindDry) < delta &&
                Abs(HeadwindWet - e.HeadwindWet) < delta &&
                Abs(TailwindDry - e.TailwindDry) < delta &&
                Abs(TailwindWet - e.TailwindWet) < delta &&
                Abs(BothReversersDry - e.BothReversersDry) < delta &&
                Abs(BothReversersWet - e.BothReversersWet) < delta &&
                Abs(Speed5Knots - e.Speed5Knots) < delta;
        }
    }
}
