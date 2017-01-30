using System;

namespace QSP.Utilities
{
    public static class ExceptionHelpers
    {
        public static void Ensure<TException>(bool condition)
            where TException : Exception, new()
        {
            if (!condition) throw new TException();
        }

        public static void IgnoreException(Action action)
        {
            Throws(action);
        }

        public static bool Throws(Action action)
        {
            try
            {
                action();
                return false;
            }
            catch
            {
                return true;
            }
        }

        public static T DefaultIfThrows<T>(Func<T> func)
        {
            try
            {
                return func();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
