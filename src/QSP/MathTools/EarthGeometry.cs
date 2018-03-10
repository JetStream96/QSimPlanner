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
        /// <exception cref="Exception"></exception>
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
        /// Calculates the great circle path from x to y, and returns heading departing x.
        /// x and y should be unique.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static double TrueHeading(ICoordinate x, ICoordinate y)
        {
            if (x.LatLonEquals(y)) throw new ArgumentException();
            return TrueHeading(GetW(x.ToVector3D(), y.ToVector3D()), x);
        }

        /// <summary>
        /// Calculates the true heading of the given vector, at the given coordinate.
        /// Return value is larger than 0 and smaller or equal to 360. Note: If v is
        /// orthogonal to the earth surface, the return value may be any number 
        /// in that range.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static double TrueHeading(Vector3D v, ICoordinate c)
        {
            if (v.R == 0) throw new ArgumentException("Length of v cannot be 0.");

            if (c.Lat >= 90.0) return 180;
            if (c.Lat <= -90.0) return 360;

            // Normal vector to tangent plane of earth at c
            var normal = c.ToVector3D();

            // Projection of v onto the tangent plane
            var projection = ProjectOnPlane(v, normal);

            if (projection.R == 0) return 360;

            // vector that points towards north on the tangent plane
            var north = ProjectOnPlane(new Vector3D(0, 0, 1), normal).Normalize();

            var east = (-normal).Cross(new Vector3D(0, 0, 1)).Normalize();

            // Calculate angle
            var x = north.Dot(projection);
            var y = east.Dot(projection);
            var heading = Angles.ToDegree(Atan2(y, x));
            var headingDeg = heading.Mod(360);
            return headingDeg == 0.0 ? 360.0 : headingDeg;

            Vector3D ProjectOnPlane(Vector3D p, Vector3D unitNormalVec)
            {
                return p - p.Dot(unitNormalVec) * unitNormalVec;
            }
        }
    }
}
