namespace QSP.MathTools.Interpolation
{
    public static class Interpolate3D
    {
        public static double Interpolate(double[] xArray, double[] yArray, double[] zArray,
                                         double x, double y, double z, double[][][] f)
        {
            int xi = Common.GetIndex(xArray, x);
            double f0 = Interpolate2D.Interpolate(yArray, zArray, y, z, f[xi]);
            double f1 = Interpolate2D.Interpolate(yArray, zArray, y, z, f[xi + 1]);
            return Interpolate1D.Interpolate(xArray[xi], xArray[xi + 1], f0, f1, x);
        }
    }
}
