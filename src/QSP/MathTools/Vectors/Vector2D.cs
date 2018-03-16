using System;

namespace QSP.MathTools.Vectors
{
    public class Vector2D : IEquatable<Vector2D>
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

        public double Theta => Math.Atan2(Y, X);

        public double R => Math.Sqrt(X * X + Y * Y);

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

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.X, -v.Y);
        }

        /// <summary>
        /// Returns the scaled vector with length 1.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public Vector2D Normalize()
        {
            var r = R;
            if (r == 0.0) throw new InvalidOperationException();
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
            return v != null && v.X == X && v.Y == Y;
        }

        public bool Equals(Vector2D v, double delta)
        {
            return v != null && (this - v).R <= delta;
        }
    }
}
