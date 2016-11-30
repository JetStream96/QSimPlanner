namespace QSP.MathTools.Interpolation
{
    public static class Interpolate3D
    {
        public static double Interpolate(double[] xArray, double[] yArray, double[] zArray, 
            double[][][] f, double x, double y, double z)
        {
            int xi = Common.GetIndex(xArray, x);
            double f0 = Interpolate2D.Interpolate(yArray, zArray, f[xi], y, z);
            double f1 = Interpolate2D.Interpolate(yArray, zArray, f[xi + 1], y, z);
            return Interpolate1D.Interpolate(xArray[xi], xArray[xi + 1], f0, f1, x);
        }
    }
}
