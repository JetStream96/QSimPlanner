using QSP.MathTools.Interpolation;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{

    public class AlternateThrustTable
    {
        private double[] FullThrustWeight;
        private double[] Dry;
        private double[] Wet;
        private double[] Climb;

        public AlternateThrustTable(double[] fullThrustWeight, 
            double[] dryWeight, 
            double[] wetWeight, 
            double[] climbWeight)
        {
            this.FullThrustWeight = fullThrustWeight;
            this.Dry = dryWeight;
            this.Wet = wetWeight;
            this.Climb = climbWeight;
        }

        public enum WeightProperty
        {
            Dry,
            Wet,
            Climb
        }

        /// <summary>
        /// Gets the corresponding full thrust limit weight, for the given condition (dry/wet/climb).
        /// </summary>
        /// <param name="weight">Field limit weight for dry/wet runway, or the climb limit weight.</param>
        /// <param name="para">Specifies the type of parameter of "weight".</param>
        public double EquivalentFullThrustWeight(double weight, WeightProperty para)
        {
            switch (para)
            {
                case WeightProperty.Dry:
                    return Interpolate1D.Interpolate(Dry, FullThrustWeight, weight);

                case WeightProperty.Wet:
                    return Interpolate1D.Interpolate(Wet, FullThrustWeight, weight);

                default:
                    //i.e. Climb
                    return Interpolate1D.Interpolate(Climb, FullThrustWeight, weight);
            }
        }

        public double CorrectedLimitWeight(double fullRatedWt, WeightProperty para)
        {
            switch (para)
            {
                case WeightProperty.Dry:
                    return Interpolate1D.Interpolate(FullThrustWeight, Dry, fullRatedWt);

                case WeightProperty.Wet:
                    return Interpolate1D.Interpolate(FullThrustWeight, Wet, fullRatedWt);

                default:
                    //i.e. Climb
                    return Interpolate1D.Interpolate(FullThrustWeight, Climb, fullRatedWt);
            }
        }
    }
}
