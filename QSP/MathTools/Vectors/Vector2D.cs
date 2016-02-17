using System;

namespace QSP
{
    public class Vector2D
    {
        public double x { get; private set; }
        public double y { get; private set; }

        //Using these relations:
        //x=rcos(theta)
        //y=rsin(theta)

        public Vector2D() : this(0.0, 0.0) { }

        public Vector2D(Vector2D item) : this(item.x, item.y) { }

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

        public static Vector2D PolarCoords(double r, double theta)
        {
            return new Vector2D(r * Math.Cos(theta), r * Math.Sin(theta));
        }

        public void Add(Vector2D v)
        {
            x += v.x;
            y += v.y;
        }

        public void Subtract(Vector2D v)
        {
            x -= v.x;
            y -= v.y;
        }

        public void Multiply(double c)
        {
            x *= c;
            y *= c;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x + v2.x, v1.y + v2.y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.x - v2.x, v1.y - v2.y);
        }

        public static Vector2D operator *(Vector2D v, double c)
        {
            return new Vector2D(v.x * c, v.y * c);
        }

        public static Vector2D operator *(double c, Vector2D v)
        {
            return v * c;
        }

        public Vector2D Normalize()
        {
            return new Vector2D(this) * (1.0 / r);
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
