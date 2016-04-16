using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public static class Stacks
    {
        public static ReadOnlyStack<T> AsReadOnly<T>(this Stack<T> item)
        {
            return new ReadOnlyStack<T>(item);
        }
    }
}
