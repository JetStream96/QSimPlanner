using QSP.LibraryExtension.Sets;
using System.Collections;
using System.Collections.Generic;

namespace QSP.RouteFinding.Containers.CountryCode
{
    public class CountryCodeCollection : IReadOnlySet<int>
    {
        private ReadOnlySet<int> codes;

        public CountryCodeCollection() : this(new int[0]) { }

        public CountryCodeCollection(IEnumerable<int> codes)
        {
            this.codes = new ReadOnlySet<int>(new HashSet<int>(codes));
        }

        public int Count => codes.Count;

        public bool Contains(int code) => codes.Contains(code);

        public IEnumerator<int> GetEnumerator() => codes.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
