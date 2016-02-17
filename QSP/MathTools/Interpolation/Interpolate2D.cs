namespace QSP.MathTools.Interpolation
{
    public static class Interpolate2D
    {
        public static double Interpolate(double[] xArray, int XIndex,
                                         double[] yArray, int YIndex,
                                         double x, double y, double[][] f)
        {
            double result1 = Interpolate1D.Interpolate(yArray, f[XIndex], YIndex, y);
            double result2 = Interpolate1D.Interpolate(yArray, f[XIndex + 1], YIndex, y);

            return Interpolate1D.Interpolate(xArray[XIndex], xArray[XIndex + 1],
                                             result1, result2, x);
        }

        public static double Interpolate(double[] xArray, double[] yArray,
                                         double x, double y, double[][] f)
        {
            return Interpolate(xArray, Common.GetIndex(xArray, x),
                               yArray, Common.GetIndex(yArray, y),
                               x, y, f);
        }
    }
}
