using System;

namespace QSP
{
    public class Vector3D : IEquatable<Vector3D>
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        // Using these relations:
        // X = R * sin(Phi) * cos(Theta)
        // Y = R * sin(Phi) * sin(Theta)
        // Z = R * cos(Phi)

        public Vector3D(Vector3D item) : this(item.X, item.Y, item.Z) { }

        public Vector3D(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /// <summary>
        /// The angle is larger or equal to 0, 
        /// and smaller or equal to PI.
        /// </summary>
        public double Phi
        {
            get { return Math.Acos(Z / R); }
        }

        public double Theta
        {
            get { return Math.Atan2(Y, X); }
        }

        public double R
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public static Vector3D FromSphericalCoords(
            double r, double phi, double theta)
        {
            double RSinPhi = r * Math.Sin(phi);

            return new Vector3D(
                RSinPhi * Math.Cos(theta),
                RSinPhi * Math.Sin(theta),
                r * Math.Cos(phi));
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3D operator *(Vector3D v, double c)
        {
            return new Vector3D(v.X * c, v.Y * c, v.Z * c);
        }

        public static Vector3D operator *(double c, Vector3D v)
        {
            return v * c;
        }

        /// <exception cref="InvalidOperationException"></exception>
        public Vector3D Normalize()
        {
            var len = R;
            if (len == 0.0) throw new InvalidOperationException();
            return new Vector3D(this) * (1.0 / len);
        }

        /// <summary>
        /// Inner product.
        /// </summary>
        public double Dot(Vector3D v)
        {
            return v.X * X + v.Y * Y + v.Z * Z;
        }

        /// <summary>
        /// Cross product.
        /// </summary>
        public Vector3D Cross(Vector3D v)
        {
            return new Vector3D(
                Y * v.Z - Z * v.Y,
                Z * v.X - X * v.Z,
                X * v.Y - Y * v.X);
        }

        public bool Equals(Vector3D other)
        {
            return other != null &&
                X == other.X &&
                Y == other.Y &&
                Z == other.Z;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }
    }
}
