using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QSP.TOPerfCalculation.Boeing.PerfData;

namespace QSP.TOPerfCalculation.Boeing
{
    public class TOParameters
    {
        public double RwyLengthMeter { get; private set; }
        public double RwyElevationFt { get; private set; }
        public double RwyHeading { get; private set; }
        public double RwySlope { get; private set; }
        public double WindHeading { get; private set; }
        public double WindSpeed { get; private set; }
        public double OatCelsius { get; private set; }
        public double QNH { get; private set; }
        public bool SurfaceWet { get; private set; }
        public double WeightKg { get; private set; }
        public ThrustRatingOption ThrustRating { get; private set; }
        public AntiIceOption AntiIce { get; private set; }
        public bool PacksOn { get; private set; }
        public int FlapsIndex { get; private set; }

        public TOParameters(
            double RwyLengthMeter,
            double RwyElevationFt,
            double RwyHeading,
            double RwySlope,
            double WindHeading,
            double WindSpeed,
            double OatCelsius,
            double QNH,
            bool SurfaceWet,
            double WeightKg,
            ThrustRatingOption ThrustRating,
            AntiIceOption AntiIce,
            bool PacksOn,
            int FlapsIndex)
        {
            this.RwyLengthMeter = RwyLengthMeter;
            this.RwyElevationFt = RwyElevationFt;
            this.RwyHeading = RwyHeading;
            this.RwySlope = RwySlope;
            this.WindHeading = WindHeading;
            this.WindSpeed = WindSpeed;
            this.OatCelsius = OatCelsius;
            this.QNH = QNH;
            this.SurfaceWet = SurfaceWet;
            this.WeightKg = WeightKg;
            this.ThrustRating = ThrustRating;
            this.AntiIce = AntiIce;
            this.PacksOn = PacksOn;
            this.FlapsIndex = FlapsIndex;
        }
    }
}
