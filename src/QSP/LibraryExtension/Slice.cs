using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QSP.LibraryExtension
{
    public class Slice<T> : IReadOnlyList<T>
    {
        private readonly IReadOnlyList<T> innerList;
        private readonly int offSet;

        public Slice(IReadOnlyList<T> list, int offSet = 0)
        {
            this.innerList = list;
            this.offSet = offSet;

            if (offSet < 0 || offSet > list.Count)
            {
                throw new ArgumentException("Offset is smaller than 0 or larger than number" + 
                    " of items in list.");
            }
        }

        public T this[int index] => innerList[index + offSet];
        public int Count => innerList.Count - offSet;
        public IEnumerator<T> GetEnumerator() => innerList.Skip(offSet).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}