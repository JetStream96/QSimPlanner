using QSP.AviationTools.Coordinates;
using QSP.MathTools.Vectors;
using QSP.RouteFinding.Data.Interfaces;
using System;
using static System.Math;

namespace QSP.MathTools
{
    /// <summary>
    /// Unit vectors are used in this class to specify positions on the sphere.
    /// The conversion between this kind of unit vector and 
    /// lat/lon is specified by method ToVector3D and ToLatLon in
    /// QSP.MathTools.Vectors.Vector3DExtension.
    /// </summary>
    public static class EarthGeometry
    {
        private static readonly Vector3D NorthPole = new LatLon(90.0, 0.0).ToVector3D();

        private static readonly Vector3D Lat0Lon0 = new LatLon(0.0, 0.0).ToVector3D();

        /// <summary>
        /// Given different v1 and v2, which are unit vectors representing two unique
        /// points on the sphere.
        /// We can get a great circle path from v1 to v2 (choose the
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
        /// Given v and v2, which are unit vectors representing two unique
        /// points on the sphere. This method returns the vector such that:
        /// (1) Tangent to the great circle route (the shorter one) from 
        ///     v to v2.
        /// (2) Normal to v.
        /// (3) Is unit vector.
        /// 
        /// If v == -v2, the chosen path is the one that goes through the 
        /// north pole, if none of v and v2 is north pole.
        /// Otherwise, the point with lat:0, lon:0.
        /// </summary>
        public static Vector3D GetW(Vector3D v, Vector3D v2)
        {
            if (v.Equals(-v2))
            {
                v2 = v.Equals(NorthPole) || v2.Equals(NorthPole) ?
                    Lat0Lon0 :
                    NorthPole;
            }

            // Now v is not parallel with v2. So their cross product is nonzero.
            var v3 = v.Cross(v2);

            // This is orthogonal to v and points to the right direction.
            return v3.Cross(v).Normalize();
        }

        /// <summary>
        /// Calculates the true heading of the given vector, at the given coordinate.
        /// </summary>
        public static double TrueHeading(Vector3D v, ICoordinate c)
        {
            // TODO: Implement this.
            throw new NotImplementedException();
        }
    }
}
