using System;

namespace QSP.Utilities
{
    public static class ExceptionHelpers
    {
        public static void IgnoreExceptions(Action action)
        {
            try
            {
                action();
            }
            catch
            { }
        }
    }
}
