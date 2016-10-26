using static QSP.MathTools.Doubles;
using static System.Math;

namespace QSP.MathTools
{
    public static class EarthGeometry
    {
        /// <summary>
        /// Given different v1 and v2, which are both unit vectors on 
        /// sphere, we can get a great circle path from v1 to v2 (choose the
        /// shortest great circle path). We walk the path by angle alpha from 
        /// v1 towards v2. This returns the point we end up with.
        /// </summary>
        public static Vector3D GetV(Vector3D v1, Vector3D v2, double alpha)
        {
            double t = v1.Dot(v2);
            var matrix = new Matrix2by2(1.0, t, t, 1.0);
            double beta = SafeAcos(t);

            var b = new Vector2D(Cos(alpha), Cos(beta - alpha));
            var a = matrix.Inverse().Multiply(b);

            return v1 * a.X + v2 * a.Y;
        }
    }
}
