namespace QSP.MathTools
{
    public static class Modulo
    {
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
