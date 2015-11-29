using System;
namespace QSP
{
    public class Vector3D
    {

        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        //Using these relations:
        //x=rsin(phi)cos(theta)
        //y=rsin(phi)sin(theta)
        //z=rcos(phi)

        public Vector3D()
        {
            x = 0;
            y = 0;
            z = 0;
        }

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

        public void SetSphericalCoords(double r, double phi, double theta)
        {
            x = r * Math.Sin(phi) * Math.Cos(theta);
            y = r * Math.Sin(phi) * Math.Sin(theta);
            z = r * Math.Cos(phi);
        }

        public Vector3D Add(Vector3D v)
        {
            return new Vector3D(v.x + x, v.y + y, v.z + z);
        }

        public Vector3D Multiply(double c)
        {
            return new Vector3D(x * c, y * c, z * c);
        }

        public Vector3D Subtract(Vector3D v)
        {
            return new Vector3D(x - v.x, y - v.y, z - v.z);
        }

        public Vector3D Normalize()
        {
            return Multiply(1 / r);
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
