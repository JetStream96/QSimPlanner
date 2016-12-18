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

        public static void IgnoreExceptions(Action action)
        {
            try
            {
                action();
            }
            catch { }
        }
    }
}
