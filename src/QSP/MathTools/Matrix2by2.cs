namespace QSP.MathTools
{

    public class Matrix2by2
    {
        public double a11;
        public double a12;
        public double a21;
        public double a22;
        //
        //    (a11  a12)
        // A= (        )
        //    (a21  a22)

        public double Det()
        {
            return a11 * a22 - a12 * a21;
        }

        public Vector2D MultiplyVector2D(Vector2D v)
        {
            return new Vector2D(a11 * v.x + a12 * v.y, a21 * v.x + a22 * v.y);
        }

        public Matrix2by2 MultiplyConst(double c)
        {
            Matrix2by2 B = new Matrix2by2();
            B.a11 = a11 * c;
            B.a12 = a12 * c;
            B.a21 = a21 * c;
            B.a22 = a22 * c;
            return B;
        }

        public Matrix2by2 InverseMatrix()
        {
            if (Det() == 0)
            {
                return null;
            }
            Matrix2by2 B = new Matrix2by2();
            B.a11 = a22;
            B.a22 = a11;
            B.a12 = (-1) * a12;
            B.a21 = (-1) * a21;
            return B.MultiplyConst(1 / Det());
        }

        public Matrix2by2()
        {
            a11 = 0;
            a12 = 0;
            a21 = 0;
            a22 = 0;
        }
    }

}
