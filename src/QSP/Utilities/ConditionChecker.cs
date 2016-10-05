using System;

namespace QSP.Utilities
{
    public static class ConditionChecker
    {
        public static void Ensure<TException>(bool condition)
            where TException : Exception, new()
        {
            if (!condition) throw new TException();
        }        
    }
}
