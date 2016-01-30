using System;

namespace QSP
{
    public class Vector3D
    {
        public double x { get; private set; }
        public double y { get; private set; }
        public double z { get; private set; }

        //Using these relations:
        //x=rsin(phi)cos(theta)
        //y=rsin(phi)sin(theta)
        //z=rcos(phi)

        public Vector3D() : this(0.0, 0.0, 0.0) { }

        public Vector3D(Vector3D item) : this(item.x, item.y, item.z) { }

        public Vector3D(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double phi
        {
            get { return Math.Asin(z / r); }
        }

        public double theta
        {
            get { return Math.Atan2(y, x); }
        }

        public double r
        {
            get { return Math.Sqrt(x * x + y * y + z * z); }
        }

        public static Vector3D GetFromSphericalCoords(double r, double phi, double theta)
        {
            double RSinPhi = r * Math.Sin(phi);

            return new Vector3D(RSinPhi * Math.Cos(theta),
                                RSinPhi * Math.Sin(theta),
                                r * Math.Cos(phi));
        }

        public void Add(Vector3D v)
        {
            x += v.x;
            y += v.y;
            z += v.z;
        }

        public void Subtract(Vector3D v)
        {
            x -= v.x;
            y -= v.y;
            z -= v.z;
        }

        public void Multiply(double c)
        {
            x *= c;
            y *= c;
            z *= c;
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3D operator *(Vector3D v, double c)
        {
            return new Vector3D(v.x * c, v.y * c, v.z * c);
        }

        public static Vector3D operator *(double c, Vector3D v)
        {
            return v * c;
        }

        public Vector3D Normalize()
        {
            return new Vector3D(this) * (1.0 / r);
        }

        public double InnerProductWith(Vector3D v)
        {
            return v.x * x + v.y * y + v.z * z;
        }

        public Vector3D CrossProductWith(Vector3D v)
        {
            return new Vector3D(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
        }

    }
}
