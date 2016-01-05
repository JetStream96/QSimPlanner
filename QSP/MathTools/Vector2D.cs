using System;

namespace QSP
{
    public class Vector2D
    {
        public double x { get; set; }
        public double y { get; set; }

        //Using these relations:
        //x=rcos(theta)
        //y=rsin(theta)

        public Vector2D()
        {
            x = 0.0;
            y = 0.0;
        }

        public Vector2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public double theta
        {
            get { return Math.Atan2(y, x); }
        }

        public double r
        {
            get { return Math.Sqrt(x * x + y * y); }
        }

        public Vector2D PolarCoords(double r, double theta)
        {
            return new Vector2D(r * Math.Cos(theta), r * Math.Sin(theta));
        }

        public Vector2D Add(Vector2D v)
        {
            return new Vector2D(x + v.x, y + v.y);
        }

        public Vector2D Multiply(double c)
        {
            return new Vector2D(x * c, y * c);
        }

        public Vector2D Subtract(Vector2D v)
        {
            return new Vector2D(x - v.x, y - v.y);
        }

        public Vector2D Normalize()
        {
            return Multiply(1.0 / r);
        }

        public double InnerProductWith(Vector2D v)
        {
            return v.x * x + v.y * y;
        }

        public bool Equals(Vector2D v)
        {
            return (v.x == x && v.y == y);
        }
    }
}
