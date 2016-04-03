using static QSP.MathTools.Constants;

namespace QSP.MathTools
{
    public static  class Angles
    {
        public static double ToRadian(double t)
        {
            return t * PIOver180;
        }

        public static double ToDegree(double t)
        {
            return t * _180OverPI;
        }
    }
}
