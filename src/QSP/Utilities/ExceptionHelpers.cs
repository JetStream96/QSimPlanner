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

        /// <summary>
        /// Start the action and returns whether exception is thrown.
        /// </summary>
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

        /// <summary>
        /// Returns the result of func, if no exception is thrown.
        /// Otherwise, return the default value.
        /// 
        /// Usage: var x = DefaultIfThrows(() => double.Parse(str), 10);
        /// </summary>
        public static T DefaultIfThrows<T>(Func<T> func, T defaultValue = default(T))
        {
            try
            {
                return func();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
