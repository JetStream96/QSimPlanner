namespace QSP.MathTools.Interpolation
{
    public class Interpolate3D
    {
        public static double Interpolate(double[] xArray, int XIndex,
                                         double[] yArray, int YIndex,
                                         double[] zArray, int ZIndex,
                                         double x, double y, double z, double[][][] f)
        {
            double result1 = Interpolate2D.Interpolate(yArray, YIndex,
                                                       zArray, ZIndex,
                                                       y, z, f[XIndex]);

            double result2 = Interpolate2D.Interpolate(yArray, YIndex,
                                                       zArray, ZIndex,
                                                       y, z, f[XIndex + 1]);

            return Interpolate1D.Interpolate(xArray[XIndex], xArray[XIndex + 1],
                                             result1, result2, x);
        }

        public static double Interpolate(double[] xArray, double[] yArray, double[] zArray,
                                         double x, double y, double z, double[][][] f)
        {
            return Interpolate(xArray, Common.GetIndex(xArray, x),
                               yArray, Common.GetIndex(yArray, y),
                               zArray, Common.GetIndex(zArray, z),
                               x, y, z, f);
        }
    }
}
