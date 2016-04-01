using QSP.Core;
using QSP.MathTools.Interpolation;
using QSP.LibraryExtension;

namespace QSP.TOPerfCalculation.Boeing.PerfData
{
    public class AlternateThrustTable
    {
        public double[] FullThrustWeights { get; private set; }
        public double[] DryWeights { get; private set; }
        public double[] WetWeights { get; private set; }
        public double[] ClimbWeights { get; private set; }

        public AlternateThrustTable(double[] FullThrustWeights,
            double[] DryWeights,
            double[] WetWeights,
            double[] ClimbWeights)
        {
            this.FullThrustWeights = FullThrustWeights;
            this.DryWeights = DryWeights;
            this.WetWeights = WetWeights;
            this.ClimbWeights = ClimbWeights;
        }

        public enum TableType
        {
            Dry,
            Wet,
            Climb
        }

        /// <summary>
        /// Gets the corresponding full thrust limit weight, for the given condition (dry/wet/climb).
        /// </summary>
        /// <param name="weight">Field limit weight for dry/wet runway, or the climb limit weight.</param>
        public double EquivalentFullThrustWeight(double weight, TableType para)
        {
            switch (para)
            {
                case TableType.Dry:
                    return Interpolate1D.Interpolate(DryWeights, FullThrustWeights, weight);

                case TableType.Wet:
                    return Interpolate1D.Interpolate(WetWeights, FullThrustWeights, weight);

                case TableType.Climb:
                    return Interpolate1D.Interpolate(ClimbWeights, FullThrustWeights, weight);

                default:
                    throw new EnumNotSupportedException();
            }
        }

        public double CorrectedLimitWeight(double fullRatedWt, TableType para)
        {
            switch (para)
            {
                case TableType.Dry:
                    return Interpolate1D.Interpolate(FullThrustWeights, DryWeights, fullRatedWt);

                case TableType.Wet:
                    return Interpolate1D.Interpolate(FullThrustWeights, WetWeights, fullRatedWt);

                case TableType.Climb:
                    return Interpolate1D.Interpolate(FullThrustWeights, ClimbWeights, fullRatedWt);

                default:
                    throw new EnumNotSupportedException();
            }
        }

        public bool Equals(AlternateThrustTable item, double delta)
        {
            return
                DoubleArrayCompare.Equals(
                    FullThrustWeights, item.FullThrustWeights, delta) &&

                DoubleArrayCompare.Equals(
                    item.DryWeights, item.DryWeights, delta) &&

                DoubleArrayCompare.Equals(
                    item.WetWeights, item.WetWeights, delta) &&

                DoubleArrayCompare.Equals(
                    item.ClimbWeights, item.ClimbWeights, delta);
        }
    }
}

