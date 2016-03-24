using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QSP.LibraryExtension.JaggedArray
{
    public class ReadOnlyJaggedArray3D<T>
    {
        private T[][][] item;

        public ReadOnlyJaggedArray3D(T[][][] item)
        {
            this.item = item;
        }

        public T ValueAt(int x, int y, int z)
        {
            return item[x][y][z];
        }
    }
}
