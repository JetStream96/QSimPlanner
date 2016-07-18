using QSP.Utilities.Units;

namespace QSP.UI.ToLdgModule.Common
{
    public class AircraftRequest
    {
        public string Aircraft { get; private set; }
        public string Registration { get; private set; }
        public double TakeOffWeightKg { get; private set; }
        public double LandingWeightKg { get; private set; }
        public WeightUnit WtUnit { get; private set; }

        public AircraftRequest(
             string Aircraft,
             string Registration,
             double TakeOffWeightKg,
             double LandingWeightKg,
             WeightUnit WtUnit)
        {
            this.Aircraft = Aircraft;
            this.Registration = Registration;
            this.TakeOffWeightKg = TakeOffWeightKg;
            this.LandingWeightKg = LandingWeightKg;
            this.WtUnit = WtUnit;
        }
    }
}
