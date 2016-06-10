namespace QSP.MathTools.Interpolation
{
    public static class Interpolate4D
    {
        public static double Interpolate(double[] xArray, int XIndex,
                                         double[] yArray, int YIndex,
                                         double[] zArray, int ZIndex,
                                         double[] tArray, int TIndex,
                                         double x, double y, double z, double t,
                                         double[][][][] f)
        {
            double result1 = Interpolate3D.Interpolate(yArray, YIndex,
                                                       zArray, ZIndex,
                                                       tArray, TIndex,
                                                       y, z, t, f[XIndex]);

            double result2 = Interpolate3D.Interpolate(yArray, YIndex,
                                                       zArray, ZIndex,
                                                       tArray, TIndex,
                                                       y, z, t, f[XIndex + 1]);

            return Interpolate1D.Interpolate(xArray[XIndex], xArray[XIndex + 1],
                                             result1, result2, x);
        }

        public static double Interpolate(double[] xArray,
                                         double[] yArray,
                                         double[] zArray,
                                         double[] tArray,
                                         double x, double y, double z, double t,
                                         double[][][][] f)
        {
            return Interpolate(xArray, Common.GetIndex(xArray, x),
                               yArray, Common.GetIndex(yArray, y),
                               zArray, Common.GetIndex(zArray, z),
                               tArray, Common.GetIndex(tArray, t),
                               x, y, z, t, f);
        }
    }
}
