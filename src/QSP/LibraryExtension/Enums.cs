using System;
using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public static class Enums
    {
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}