namespace QSP.MathTools
{
    public static class Modulo
    {
        /// <summary>
        /// Given a positive x, the result is larger or equal to 0 and smaller 
        /// than x.
        /// </summary>
        public static int Mod(this int item, int x)
        {
            return (item % x + x) % x;
        }

        public static double Mod(this double item, int x)
        {
            return (item % x + x) % x;
        }

        public static double Mod(this double item, double x)
        {
            return (item % x + x) % x;
        }
    }
}
