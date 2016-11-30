using System;

namespace QSP.MathTools
{
    public static class Angles
    {
        public const double PIOver180 = Math.PI / 180.0;
        public const double _180OverPI = 180.0 / Math.PI;

        public static double ToRadian(double t) => t * PIOver180;
        public static double ToDegree(double t) => t * _180OverPI;
    }
}
