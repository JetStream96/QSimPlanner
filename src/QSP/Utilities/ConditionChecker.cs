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

        public static void ThrowWhenNegative<TException>(int x)
            where TException : Exception, new()
        {
            if (x < 0)
            {
                throw new TException();
            }
        }

        public static void Ensure<TException>(bool x)
            where TException : Exception, new()
        {
            ThrowWhenFalse<TException>(x);
        }

        public static void ThrowWhenFalse<TException>(bool x)
            where TException : Exception, new()
        {
            if (x == false)
            {
                throw new TException();
            }
        }

        public static void ThrowWhenTrue<TException>(bool x)
            where TException : Exception, new()
        {
            if (x)
            {
                throw new TException();
            }
        }
    }
}
