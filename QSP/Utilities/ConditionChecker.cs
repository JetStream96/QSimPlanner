using System;

namespace QSP.Utilities
{
    public static class ConditionChecker
    {
        public static void ThrowWhenNull<T>(T obj) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void ThrowWhenNegative(int x)
        {
            if (x < 0)
            {
                throw new ArgumentException();
            }
        }

        public static void Ensure(bool x)
        {
            if (x == false)
            {
                throw new ArgumentException();
            }
        }

        public static void ThrowWhenTrue(bool x)
        {
            if (x)
            {
                throw new ArgumentException();
            }
        }
    }
}
