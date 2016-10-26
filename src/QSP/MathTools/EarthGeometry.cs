using QSP.AviationTools.Coordinates;
using QSP.MathTools.Vectors;
using System;
using static QSP.MathTools.Doubles;
using static System.Math;

namespace QSP.MathTools
{
    public static class EarthGeometry
    {
        private static readonly Vector3D NorthPole =
            new LatLon(90.0, 0.0).ToVector3D();

        private static readonly Vector3D Lat0Lon0 =
            new LatLon(0.0, 0.0).ToVector3D();

        /// <summary>
        /// Given different v1 and v2, which are both unit vectors on 
        /// sphere, we can get a great circle path from v1 to v2 (choose the
        /// shortest great circle path). We walk the path by angle alpha from 
        /// v1 towards v2. This returns the point we end up with, which is
        /// an unit vector.
        /// If v1 == v2, an exception is thrown.
        /// If v1 == -v2, the chosen path is the one that goes through 
        /// the north pole, if none of v1, v2 is north pole. Otherwise, 
        /// the point with lat:0, lon:0.
        /// </summary>
        public static Vector3D GetV(Vector3D v1, Vector3D v2, double alpha)
        {
            double t = v1.Dot(v2);
            if (t >= 1.0) throw new ArgumentException();
            if (t <= -1.0)
            {
                if (v1.Equals(NorthPole) || v2.Equals(NorthPole))
                {
                    return GetV(v1, Lat0Lon0, alpha);
                }

                return GetV(v1, NorthPole, alpha);
            }

            var matrix = new Matrix2by2(1.0, t, t, 1.0);
            double beta = Acos(t);

            var b = new Vector2D(Cos(alpha), Cos(beta - alpha));
            var a = matrix.Inverse().Multiply(b);

            return v1 * a.X + v2 * a.Y;
        }

        /// <summary>
        /// Given v, v1, v2, all are unit vectors on the sphere, and
        /// all on the same plane. This method returns the vector such that:
        /// (1) Tangent to the great circle route (the shorter one) from 
        ///     v1 to v2.
        /// (2) Normal to v.
        /// (3) Is unit vector.
        /// It's required that v1 != v2. 
        /// If v1 == -v2, then any unit vector v is a valid input, and 
        /// </summary>
        public static Vector3D GetW(Vector3D v, Vector3D v1, Vector3D v2)
        {
            var v3 = v1.Cross(v2);
            v3 = v3.Normalize();
            return v3.Cross(v);
        }
    }
}
