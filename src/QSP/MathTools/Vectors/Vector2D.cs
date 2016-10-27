using System;

namespace QSP.MathTools.Vectors
{
    public class Vector2D
    {
        public double X { get; }
        public double Y { get; }

        // Using these relations:
        // x = r * cos(theta)
        // y = r * sin(theta)

        public Vector2D(Vector2D item) : this(item.X, item.Y) { }

        public Vector2D(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public double theta => Math.Atan2(Y, X);

        public double r => Math.Sqrt(X * X + Y * Y);

        public static Vector2D PolarCoords(double r, double theta)
        {
            return new Vector2D(r * Math.Cos(theta), r * Math.Sin(theta));
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator *(Vector2D v, double c)
        {
            return new Vector2D(v.X * c, v.Y * c);
        }

        public static Vector2D operator *(double c, Vector2D v)
        {
            return v * c;
        }

        public Vector2D Normalize()
        {
            return new Vector2D(this) * (1.0 / r);
        }

        /// <summary>
        /// Inner product.
        /// </summary>
        public double Dot(Vector2D v)
        {
            return v.X * X + v.Y * Y;
        }

        public bool Equals(Vector2D v)
        {
            return v.X == X && v.Y == Y;
        }
    }
}
