using QSP.MathTools;
using static QSP.MathTools.InterpolationOld;

namespace QSP.TakeOffPerfCalculation
{

    public class AlternateThrustTable
    {

        private double[] FullThrustWeight;
        private ArrayOrder Order;
        private double[] Dry;
        private double[] Wet;

        private double[] Climb;

        public AlternateThrustTable(double[] fullThrustWeight,double[] dryWeight, double[] wetWeight, double[] climbWeight)
            :this(fullThrustWeight,fullThrustWeight.GetOrder(),dryWeight,wetWeight,climbWeight )
        {
        }

        public AlternateThrustTable(double[] fullThrustWeight, ArrayOrder arrayOrder, double[] dryWeight, double[] wetWeight, double[] climbWeight)
        {
            this.FullThrustWeight = fullThrustWeight;
            this.Order = arrayOrder;
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
                    return Interpolate(Dry, weight, FullThrustWeight, Order);
                case WeightProperty.Wet:
                    return Interpolate(Wet, weight, FullThrustWeight, Order);
                default:
                    //i.e. Climb
                    return Interpolate(Climb, weight, FullThrustWeight, Order);
            }
        }

        public double CorrectedLimitWeight(double fullRatedWt, WeightProperty para)
        {
            switch (para)
            {
                case WeightProperty.Dry:
                    return Interpolate(FullThrustWeight, fullRatedWt, Dry, Order);
                case WeightProperty.Wet:
                    return Interpolate(FullThrustWeight, fullRatedWt, Wet, Order);
                default:
                    //i.e. Climb
                    return Interpolate(FullThrustWeight, fullRatedWt, Climb, Order);
            }
        }

    }

}
