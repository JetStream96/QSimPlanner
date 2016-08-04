using System.Collections.Generic;
using System.Linq;

namespace QSP.RouteFinding.Containers.CountryCode
{
    public class CountryCodeCollection
    {
        private List<int> codeList;
        private HashSet<int> codeSet;

        public CountryCodeCollection() : this(new int[0]) { }

        public CountryCodeCollection(IEnumerable<int> codes)
        {
            if (codes.Count() <= 10)
            {
                codeList = codes.ToList();
            }
            else
            {
                codeSet = new HashSet<int>(codes);
            }
        }

        public bool Contains(int code)
        {
            if (codeList == null)
            {
                return codeSet.Contains(code);
            }
            else
            {
                return codeList.Contains(code);
            }
        }
    }
}
