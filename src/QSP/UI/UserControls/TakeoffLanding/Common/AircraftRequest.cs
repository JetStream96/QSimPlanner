using QSP.Utilities.Units;

namespace QSP.UI.UserControls.TakeoffLanding.Common
{
    public class AircraftRequest
    {
        public string Aircraft { get; private set; }
        public string Registration { get; private set; }
        public double TakeOffWeightKg { get; private set; }
        public double LandingWeightKg { get; private set; }
        public double ZfwKg { get; private set; }
        public WeightUnit WtUnit { get; private set; }

        public AircraftRequest(
             string Aircraft,
             string Registration,
             double TakeOffWeightKg,
             double LandingWeightKg,
             double ZfwKg,
             WeightUnit WtUnit)
        {
            this.Aircraft = Aircraft;
            this.Registration = Registration;
            this.TakeOffWeightKg = TakeOffWeightKg;
            this.LandingWeightKg = LandingWeightKg;
            this.ZfwKg = ZfwKg;
            this.WtUnit = WtUnit;
        }
    }
}
