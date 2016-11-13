namespace QSP.MathTools.Interpolation
{
    public static class Interpolate2D
    {
        /// <summary>
        /// Computes f(x,y) using interpolation.
        /// </summary>
        /// <param name="f00">f(x0,y0)</param>
        /// <param name="f01">f(x0,y1)</param>
        /// <param name="f10">f(x1,y0)</param>
        /// <param name="f11">f(x1,y1)</param>
        public static double Interpolate(
            double x0, double x1, double x,
            double y0, double y1, double y,
            double f00, double f01, double f10, double f11)
        {
            double p = Interpolate1D.Interpolate(x0, x1, f00, f10, x);
            double q = Interpolate1D.Interpolate(x0, x1, f01, f11, x);

            return Interpolate1D.Interpolate(y0, y1, p, q, y);
        }

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
