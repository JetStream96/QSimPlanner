using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.JaggedArray
{
    public class ReadOnlyJaggedArray2D<T>
    {
        private T[][] item;

        public ReadOnlyJaggedArray2D(T[][] item)
        {
            this.item = item;
        }

        public T ValueAt(int x, int y)
        {
            return item[x][y];
        }
    }
}
