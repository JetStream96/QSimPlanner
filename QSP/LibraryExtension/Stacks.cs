using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
