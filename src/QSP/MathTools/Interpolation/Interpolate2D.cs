namespace QSP.MathTools.Interpolation
{
    public static class Interpolate2D
    {
        public static double Interpolate(double[] xArray, double[] yArray, double[][] f, 
            double x, double y)
        {
            int xi = Common.GetIndex(xArray, x);
            double f0 = Interpolate1D.Interpolate(yArray, f[xi], y);
            double f1 = Interpolate1D.Interpolate(yArray, f[xi + 1], y);
            return Interpolate1D.Interpolate(xArray[xi], xArray[xi + 1], f0, f1, x);
        }
    }
}
